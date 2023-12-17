using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ICacheService cacheService) : base(cacheService)
    {
    }
}