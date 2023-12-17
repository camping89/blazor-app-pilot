using BlazorApp.Application.Caching;
using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories.Implementations;

public class ClientRepository : BaseRepository<Client>, IClientRepository
{
    public ClientRepository(ICacheService cacheService) : base(cacheService)
    {
    }
}