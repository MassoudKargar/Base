namespace Base.Infrastructure.Caching.Redis.DI;
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"><see cref="RedisConfiguration"/> name="RedisConfiguration"/> this class set to appsettings.json file from name "RedisConfiguration"</param>
    public static void AddRedisService(this IServiceCollection services, IConfiguration _configuration)
    {
        services.Configure<RedisConfiguration>(options => _configuration.GetSection("RedisConfiguration"));
        services.Configure<RedisSetting>(options => _configuration.GetSection("RedisSetting"));
        var redisConfiguration = _configuration.GetSection("RedisConfiguration").Get<RedisConfiguration>();
        var redisSetting = _configuration.GetSection("RedisSetting").Get<RedisSetting>();
        var options = new ConfigurationOptions
        {
            AllowAdmin = false,
            Ssl = false,
            KeepAlive = redisSetting.KeepAlive,
            SyncTimeout = redisSetting.SyncTimeout,
            ConnectTimeout = redisSetting.ConnectTimeOut,
            ConnectRetry = redisSetting.ConnectRetry,
            ReconnectRetryPolicy = new LinearRetry(redisSetting.ReconnectRetryPolicy),
            AbortOnConnectFail = redisSetting.AbortOnConnectFail,
        };
        for (int i = 0; i < redisConfiguration.EndPoints.Length; i++)
        {
            var items = redisConfiguration.EndPoints[i];
            var host = items.Split(',')[0].Split(':')[0];
            var port = items.Split(',')[0].Split(':')[1];
            options.EndPoints.Add(host, int.Parse(port));
        }
        options.Password = redisConfiguration.Password;
        services.AddSingleton<IDistributedCacheRepository, DistributedCacheRepository>();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options));
        services.AddStackExchangeRedisCache(op => op.ConfigurationOptions = options);
    }
}