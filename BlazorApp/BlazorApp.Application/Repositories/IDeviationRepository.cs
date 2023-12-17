using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories;

public interface IDeviationRepository : IBaseRepository<Deviation>
{
    Task<Deviation?> GetByShiftId(int shiftId);
}