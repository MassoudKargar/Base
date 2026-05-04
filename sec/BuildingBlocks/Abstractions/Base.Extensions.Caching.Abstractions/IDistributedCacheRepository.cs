namespace Heris.Infrastructure.Caching;

public interface IDistributedCacheRepository
{
    Task<T?> GetAsync<T>(string cacheKey);
    Task<IDictionary<string, T?>> GetByPrefixAsync<T>(string prefix);
    Task<int> GetCountAsync(string prefix = "");
    Task<TimeSpan> GetExpirationAsync(string cacheKey);
    Task RemoveAllAsync(IEnumerable<string> cacheKeys);
    Task RemoveAsync(string cacheKey);
    Task RemoveByPrefixAsync(string prefix);
    Task SetAllAsync<T>(IDictionary<string, T> value, TimeSpan expiration);
    Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration);
    Task<bool> ExistsAsync(string cacheKey);
}
