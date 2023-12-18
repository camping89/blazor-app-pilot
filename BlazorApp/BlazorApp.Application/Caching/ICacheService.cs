namespace BlazorApp.Application.Caching;


public interface ICacheService
{
    string GetCacheName<T>(string value);

    Task<T>       Get<T>(string         cacheName,   Func<Task<T>>     loader, CancellationToken cancellationToken = default) where T : class;
    Task<T?>      Get<T>(string         key,         CancellationToken cancellationToken = default) where T : class;
    Task<List<T>> GetByPrefix<T>(string prefix = "", CancellationToken cancellationToken = default) where T : class;

    Task Set<T>(string key, T value, CancellationToken cancellationToken = default) where T : class;

    Task Remove(string         key,    CancellationToken cancellationToken = default);
    Task Remove<T>(string      key,    CancellationToken cancellationToken = default);
}