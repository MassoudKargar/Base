namespace Base.Infrastructure.Caching.Redis.DI;

public class RedisConfiguration
{
    public TimeSpan ReserveChargeTimeout { get; set; }
    public TimeSpan ChargeServiceTimeout { get; set; }
    public string[] EndPoints { get; set; }
    public string Password { get; set; }
    public string ClientName { get; set; }
    public bool Ssl { get; set; }
    public bool AllowAdmin { get; set; }
    public int DataBaseId { get; set; }
}
