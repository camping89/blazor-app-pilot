using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories.Interfaces;

public interface IDeviationRepository : IBaseRepository<Deviation>
{
    Task<Deviation?> GetByShiftId(int shiftId);
}