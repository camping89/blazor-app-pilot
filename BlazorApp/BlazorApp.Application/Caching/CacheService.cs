using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BlazorApp.Application.Caching;

public class CacheService : ICacheService
{
    private static ConcurrentDictionary<string, byte> CacheKeys = new();

    private readonly int               _defaultExpirationInMinutes;
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache           = distributedCache;
        _defaultExpirationInMinutes = 30;
    }

    public string GetCacheName<T>(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return $"{typeof(T).Name}";
        var cacheName = $"{typeof(T).Name}:{value}";
        return cacheName;
    }

    public async Task<T> Get<T>(string key, Func<Task<T>> loader, CancellationToken cancellationToken = default) where T : class
    {
        var cacheKey = GetCacheName<T>(key);

        if (loader == null) throw new ArgumentNullException(nameof(loader));

        var cachedValue = await Get<T>(cacheKey, cancellationToken);
        if (cachedValue is not null) return cachedValue;

        var value = await loader();
        await Set(key, value, cancellationToken);

        return value;
    }

    public async Task<T?> Get<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var cacheKey    = GetCacheName<T>(key);
        var cachedValue = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
        if (cachedValue is null) return null;

        return JsonConvert.DeserializeObject<T>(cachedValue);
    }

    public async Task Set<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        var    cacheKey    = GetCacheName<T>(key);
        string cachedValue = JsonConvert.SerializeObject(value);
        await _distributedCache.SetStringAsync(cacheKey,
                                               cachedValue,
                                               new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_defaultExpirationInMinutes) },
                                               cancellationToken);

        CacheKeys.TryAdd(cacheKey, 0);
    }

    public async Task Remove(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
        CacheKeys.TryRemove(key, out byte _);
    }

    public async Task Remove<T>(string key, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetCacheName<T>(key);
        await Remove(cacheKey, cancellationToken);
    }
    
    public async Task<List<T>> GetByPrefix<T>(string prefix = "", CancellationToken cancellationToken = default) where T : class
    {
        var cacheKey = GetCacheName<T>(prefix).ToLower();
        var keys    = CacheKeys.Keys.Where(key => key.ToLower().StartsWith(cacheKey));
        var result = new List<T>();
        foreach (var key in keys)
        {
            var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (cachedValue is null) continue;

            var entity = JsonConvert.DeserializeObject<T>(cachedValue);
            if (entity != null) result.Add(entity);
        }

        return result;
    }
}