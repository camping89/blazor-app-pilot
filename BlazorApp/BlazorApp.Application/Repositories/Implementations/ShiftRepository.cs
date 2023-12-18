using BlazorApp.Application.Caching;
using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories.Implementations;

public class ShiftRepository: BaseRepository<Shift>, IShiftRepository
{
    public ShiftRepository(ICacheService cacheService)
        : base(cacheService)
    {
    }
    
    // custom method -> IShiftRepository
    public async Task<List<Shift>> GetByEmployeeId(int employeeId)
    {
        var shifts = await Get();

        return shifts.Where(shift => shift.EmployeeId == employeeId).ToList();
    }

    public async Task<List<Shift>> GetByClientId(int clientId)
    {
        var shifts = await Get();
        return shifts.Where(shift => shift.ClientId == clientId).ToList();
    }
}