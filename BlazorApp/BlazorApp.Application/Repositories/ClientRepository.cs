using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories;

public class ClientRepository : BaseRepository<Client>, IClientRepository
{
    public ClientRepository(ICacheService cacheService) : base(cacheService)
    {
    }
}