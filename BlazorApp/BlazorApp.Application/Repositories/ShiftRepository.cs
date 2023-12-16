using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories;

public class ShiftRepository: BaseRepository<Shift>
{
    public ShiftRepository(ICacheService cacheService)
        : base(cacheService)
    {
    }
    
    // custom method -> IShiftRepository
}