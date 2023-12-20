using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories.Interfaces;

public interface IDeviationRepository : IBaseRepository<Deviation>
{
    Task<List<Deviation>> GetByShiftId(int shiftId);
}