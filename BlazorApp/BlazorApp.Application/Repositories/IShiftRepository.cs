using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories;

public interface IShiftRepository : IBaseRepository<Shift>
{
    Task<IList<Shift>> GetByEmployeeId(int employeeId);
}