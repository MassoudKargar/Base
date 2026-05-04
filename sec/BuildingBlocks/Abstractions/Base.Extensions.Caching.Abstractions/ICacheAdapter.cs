namespace Base.Extensions.Caching.Abstractions;
public interface ICacheAdapter
{
    void Add<TInput>(string key, TInput obj, DateTime? absoluteExpiration, TimeSpan? slidingExpiration);
    TOutput Get<TOutput>(string key);
    void RemoveCache(string key);
}
