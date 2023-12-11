using BlazorApp.Share.Enums;
using BlazorApp.Share.Models;

namespace BlazorApp.Data;

public class EmployService
{
    public async Task<IList<Employee>> GetEmployees()
    {
        return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new Employee
        {
            Id = index,
            DaysAvailable = 10,
            TranzitMin = 5,
            VacationDays = 12,
            Shifts = new List<Shift>
            {
                new()
                {
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    EmployeeId = index,
                    ClientId = index,
                    Title = $"Client {index}",
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    StartTime = new TimeOnly(8 + index,0,0),
                    EndTime = new TimeOnly(12 + index,0,0,0),
                    Status = Status.Approved
                }
            }
        }).ToList());
    }
}