using BlazorApp.Share.Entities;

namespace BlazorApp.Application.Repositories.Interfaces;

public interface IShiftRepository : IBaseRepository<Shift>
{
    Task<List<Shift>> GetByEmployeeId(int employeeId);
    Task<List<Shift>> GetByClientId(int clientId);
}