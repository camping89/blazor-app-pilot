using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories;

public class ShiftRepository: BaseRepository<Shift>, IShiftRepository
{
    public ShiftRepository(ICacheService cacheService)
        : base(cacheService)
    {
    }
    
    // custom method -> IShiftRepository
    public async Task<IList<Shift>> GetByEmployeeId(int employeeId)
    {
        var shifts = await Get();

        return shifts.Where(shift => shift.EmployeeId == employeeId).ToList();
    }
}