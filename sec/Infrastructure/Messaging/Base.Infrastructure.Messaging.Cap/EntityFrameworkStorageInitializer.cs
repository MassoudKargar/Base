using NpgsqlTypes;

namespace Base.Infrastructure.Messaging.Cap;

public class EntityFrameworkStorageInitializer(
    ILogger<PostgreSqlStorageInitializer> logger,
    IOptions<PostgreSqlOptions> options,
    IOptions<CapOptions> capOptions)
    : IStorageInitializer
{
    private readonly ILogger _logger = logger;

    public string GetLockTableName()
    {
        return $"{Common.Schema}.{Common.LockTableName}";
    }

    public string GetLockTableNamePostgre()
    {
        return $"{Common.LockTableName}";
    }

    public virtual string GetPublishedTableName()
    {
        return $"{Common.Schema}.{Common.PublishedTableName}";
    }

    public virtual string GetReceivedTableName()
    {
        return $"{Common.Schema}.{Common.ReceivedTableName}";
    }


    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return;

        var sql = CreateDbTablesPostgreSqlScript(Common.Schema);
        var connection = new NpgsqlConnection(options.Value.ConnectionString);
        await using var _ = connection.ConfigureAwait(false);
        object[] sqlParams =
        {
            new NpgsqlParameter("@PubKey", $"publish_retry_{capOptions.Value.Version}"),
            new NpgsqlParameter("@RecKey", $"received_retry_{capOptions.Value.Version}"),
            new NpgsqlParameter("@LastLockTime", DateTime.MinValue){ NpgsqlDbType = NpgsqlDbType.Timestamp },
        };
        await ExecuteNonQueryAsync(connection, sql, sqlParams: sqlParams).ConfigureAwait(false);

        _logger.LogDebug("Ensuring all create database tables script are applied.");
    }

    protected virtual string CreateDbTablesSqlServerScript(string schema)
    {
        // PublishedTable and ReceivedTableName are ignored
        // Will be made by ef migration 

        var batchSql =
            $@"
                IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = '{schema}')
                BEGIN
	                EXEC('CREATE SCHEMA [{schema}]')
                END;";

        if (capOptions.Value.UseStorageLock)
            batchSql += $@"
                IF OBJECT_ID(N'{GetLockTableName()}',N'U') IS NULL
                BEGIN
                CREATE TABLE {GetLockTableName()}(
	                [Key] [nvarchar](128) NOT NULL,
                    [Instance] [nvarchar](256) NOT NULL,
	                [LastLockTime] [datetime2](7) NOT NULL,
                 CONSTRAINT [PK_{GetLockTableName()}] PRIMARY KEY CLUSTERED
                (
	                [Key] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY] 
                END;
                INSERT INTO {GetLockTableName()} ([Key],[Instance],[LastLockTime]) VALUES(@PubKey,'',@LastLockTime);
                INSERT INTO {GetLockTableName()} ([Key],[Instance],[LastLockTime]) VALUES(@RecKey,'',@LastLockTime);
                ";
        return batchSql;
    }


    protected virtual string CreateDbTablesPostgreSqlScript(string schema)
    {
        var batchSql = $@"
                        DO $$ 
                        BEGIN
                            -- Create schema if it doesn't exist
                            IF NOT EXISTS (SELECT 1 FROM information_schema.schemata WHERE schema_name = '{schema}') THEN
                                EXECUTE 'CREATE SCHEMA {schema}';
                            END IF;
                        END $$;
                        
                        -- Create table if it doesn't exist in the '{schema}' schema
                        DO $$ 
                        BEGIN
                            IF NOT EXISTS (
                                SELECT 1 
                                FROM information_schema.tables 
                                WHERE table_schema = '{schema}' 
                                AND table_name = 'MessagingLock'
                            ) THEN
                                EXECUTE format(
                                    'CREATE TABLE %I.%I (
                                        %I TEXT PRIMARY KEY,
                                        %I TEXT NOT NULL DEFAULT '''',
                                        %I TIMESTAMP NOT NULL
                                    )',
                                    '{schema}', '{GetLockTableNamePostgre()}', 'Key', 'Instance', 'LastLockTime'
                                );
                            END IF;
                        END $$;
                        
                        -- Insert data into the table if it doesn't exist already
                        INSERT INTO ""{schema}"".""{GetLockTableNamePostgre()}"" (""Key"", ""Instance"", ""LastLockTime"")
                            VALUES
                                ('publish_retry_v1', '', CURRENT_TIMESTAMP),  -- PubKey value
                                ('received_retry_v1', '', CURRENT_TIMESTAMP)   -- RecKey value
                            ON CONFLICT (""Key"") DO NOTHING;  -- Ignore conflict if 'Key' already exists
                        ";


        return batchSql;

    }


    private static async Task<int> ExecuteNonQueryAsync(DbConnection connection, string sql,
     DbTransaction? transaction = null, params object[] sqlParams)
    {
        if (connection.State == ConnectionState.Closed) await connection.OpenAsync().ConfigureAwait(false);

        var command = connection.CreateCommand();
        await using var _ = command.ConfigureAwait(false);
        command.CommandType = CommandType.Text;
        command.CommandText = sql;
        foreach (var param in sqlParams) command.Parameters.Add(param);

        if (transaction != null) command.Transaction = transaction;

        return await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

}
