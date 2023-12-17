using BlazorApp.Application.Caching;
using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories.Implementations;

public class DeviationRepository : BaseRepository<Deviation>, IDeviationRepository
{
    public DeviationRepository(ICacheService cacheService) : base(cacheService)
    {
    }

    public async Task<Deviation?> GetByShiftId(int shiftId)
    {
        var deviations = await _cacheService.GetByPrefix<Deviation>();
        return deviations.FirstOrDefault(deviation => deviation.ShiftId == shiftId);
    }
}