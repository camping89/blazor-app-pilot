using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories;

public interface IBaseRepository<T> where T : Entity
{
    Task<T?>      Get(string id);
    Task<List<T>> Get();
    Task          Add(T         entity);
    Task          Update(string id, T entity);
    Task          Delete(string id);
}

public class BaseRepository<T> : IBaseRepository<T> where T : Entity
{
    private readonly ICacheService _cacheService;

    public BaseRepository(ICacheService cacheService) => _cacheService = cacheService;

    public async Task<T?> Get(string id) => await _cacheService.Get<T>(id);

    public async Task<List<T>> Get() => await _cacheService.GetByPrefix<T>();

    public async Task Add(T entity)
    {
        await _cacheService.Set(entity.Id.ToString(), entity);
    }

    public async Task Update(string id, T entity)
    {
        await _cacheService.Set(entity.Id.ToString(), entity);
    }

    public async Task Delete(string id)
    {
        await _cacheService.Remove<T>(id);
    }
}