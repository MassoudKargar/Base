namespace Base.Infrastructure.Caching.Redis.DI;
public class RedisSetting
{
    public int SyncTimeout { get; set; }
    public int ConnectTimeOut { get; set; }
    public int KeepAlive { get; set; }
    public int ConnectRetry { get; set; }
    public int ReconnectRetryPolicy { get; set; }
    public bool AbortOnConnectFail { get; set; }
}
