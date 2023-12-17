using BlazorApp.Application.Caching;
using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories.Implementations;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ICacheService cacheService) : base(cacheService)
    {
    }
}