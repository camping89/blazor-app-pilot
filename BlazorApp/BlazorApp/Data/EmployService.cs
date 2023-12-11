using BlazorApp.Application;
using BlazorApp.Share.Models;

namespace BlazorApp.Data;

public class EmployService
{
    public async Task<IList<Employee>> GetEmployees()
    {
        var testData =  new TestDataGenerator().Generate();
        foreach (var employee in testData.Employees)
        {
            var shifts = testData.Shifts.Where(shift => shift.EmployeeId == employee.Id).ToList();
            employee.Shifts = shifts;
            foreach (var shift in shifts)
            {
                shift.Deviations = testData.Deviations
                    .Where(deviation => deviation.ShiftId == shift.Id && employee.Id == deviation.EmployeeId).ToList();
                shift.Client = testData.Clients.FirstOrDefault(client => client.Id == shift.ClientId);
            }
        }

        return testData.Employees;
    }
}