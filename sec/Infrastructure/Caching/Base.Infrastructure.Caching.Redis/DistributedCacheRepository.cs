namespace Base.Infrastructure.Caching.Redis;

public class DistributedCacheRepository : IDistributedCacheRepository
{
    private Lazy<IConnectionMultiplexer> _connection;
    private readonly ConfigurationOptions _configuration;
    private readonly RedisConfiguration _config;

    private IConnectionMultiplexer Connection { get { return _connection.Value; } }
    private IDatabase Database => Connection.GetDatabase(_config.DataBaseId);
    public DistributedCacheRepository(IConfiguration config)
    {
        _config = config.GetSection("RedisConfiguration").Get<RedisConfiguration>();

        _configuration = new ConfigurationOptions()
        {
            AllowAdmin = _config.AllowAdmin,
            Password = _config.Password,
            ClientName = _config.ClientName,
            Ssl = _config.Ssl
        };

        foreach (var item in _config.EndPoints)
            _configuration.EndPoints.Add(item);

        _connection = new Lazy<IConnectionMultiplexer>(() =>
        {
            var redis = ConnectionMultiplexer.Connect(_configuration);
            return redis;
        });
    }

    public async Task<bool> ExistsAsync(string cacheKey)
    {
        return await Database.KeyExistsAsync(cacheKey);
    }

    public async Task<T?> GetAsync<T>(string cacheKey)
    {
        var jsonValue = await Database.StringGetAsync(cacheKey);
        return jsonValue.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(jsonValue!));
    }

    public async Task<IDictionary<string, T?>> GetByPrefixAsync<T>(string prefix)
    {
        var server = Connection.GetServer(Connection.GetEndPoints().First());
        var keys = server.Keys(pattern: $"{prefix}*");
        var results = new Dictionary<string, T?>();
        foreach (var key in keys)
        {
            var value = await Database.StringGetAsync(key);
            results[key] = value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(value));
        }
        return results;
    }

    public async Task<int> GetCountAsync(string prefix = "")
    {
        var server = Connection.GetServer(Connection.GetEndPoints().First());
        return await Task.FromResult(server.Keys(pattern: $"{prefix}*").Count());
    }

    public async Task<TimeSpan> GetExpirationAsync(string cacheKey)
    {
        var ttl = await Database.KeyTimeToLiveAsync(cacheKey);
        return ttl.GetValueOrDefault();
    }

    public async Task RemoveAllAsync(IEnumerable<string> cacheKeys)
    {
        var tasks = cacheKeys.Select(key => Database.KeyDeleteAsync(key));
        await Task.WhenAll(tasks);
    }

    public async Task RemoveAsync(string cacheKey)
    {
        await Database.KeyDeleteAsync(cacheKey);
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        var server = Connection.GetServer(Connection.GetEndPoints().First());
        var keys = server.Keys(pattern: $"{prefix}*");
        var tasks = keys.Select(key => Database.KeyDeleteAsync(key));
        await Task.WhenAll(tasks);
    }

    public async Task SetAllAsync<T>(IDictionary<string, T> values, TimeSpan expiration)
    {
        var tasks = values.Select(kv => Database.StringSetAsync(kv.Key, JsonSerializer.Serialize(kv.Value), expiration));
        await Task.WhenAll(tasks);
    }

    public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration = default)
    {
        var jsonValue = JsonSerializer.Serialize(cacheValue);
        if (expiration == default)
        {
            await Database.StringSetAsync(cacheKey, jsonValue);

        }
        else
        {
            await Database.StringSetAsync(cacheKey, jsonValue, expiration);
        }
    }
}
