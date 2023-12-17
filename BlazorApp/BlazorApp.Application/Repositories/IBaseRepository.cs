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
    protected static ICacheService _cacheService;

    public BaseRepository(ICacheService cacheService) => _cacheService = cacheService;

    public async Task<T?> Get(string id) => await _cacheService.Get<T>(id);

    public virtual async Task<List<T>> Get()
    {
        var data = await _cacheService.GetByPrefix<T>();
        return data;
    }

    public async Task Add(T entity)
    {
        var currentDateTime = DateTime.UtcNow;
        entity.CreatedAt = currentDateTime;
        entity.ModifiedAt = currentDateTime;
        await _cacheService.Set(entity.Id.ToString(), entity);
    }

    public async Task Update(string id, T entity)
    {
        entity.ModifiedAt = DateTime.UtcNow;
        await _cacheService.Set(entity.Id.ToString(), entity);
    }

    public async Task Delete(string id)
    {
        await _cacheService.Remove<T>(id);
    }
}