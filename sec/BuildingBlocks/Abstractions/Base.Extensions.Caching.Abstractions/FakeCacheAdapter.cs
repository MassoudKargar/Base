namespace Base.Extensions.Caching.Abstractions;

public class FakeCacheAdapter : ICacheAdapter
{
    public void Add<TInput>(string key, TInput obj, DateTime? absoluteExpiration, TimeSpan? slidingExpiration) { }

    public TOutput Get<TOutput>(string key) => default;

    public void RemoveCache(string key) { }
}