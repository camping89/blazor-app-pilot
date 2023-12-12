using BlazorApp.Application;
using BlazorApp.Share.Models;

namespace BlazorApp.Data;

public class EmployService
{
    private static IList<Employee> Employees = new List<Employee>();
    private readonly GenerateService _generateService;

    public EmployService(GenerateService generateService)
    {
        _generateService = generateService;
    }
    public async Task<IList<Employee>> GetEmployees()
    {
        if (!Employees.Any())
        {
            var testData =  _generateService.GenerateTestData();
            foreach (var employee in testData.Employees)
            {
                var shifts = testData.Shifts.Where(shift => shift.EmployeeId == employee.Id).ToList();
                employee.Shifts = shifts;
                foreach (var shift in shifts)
                {
                    shift.Deviations = new List<Deviation>
                    {
                        testData.Deviations
                            .Last(deviation => deviation.ShiftId == shift.Id && employee.Id == deviation.EmployeeId)
                    };
                    Console.WriteLine(
                        $"shift id {shift.Id}, shift Duration {shift.Duration}, deviation id {shift.Deviations.First().Id}, deviation duration {shift.Deviations.First().Duration}");
                    shift.Client = testData.Clients.FirstOrDefault(client => client.Id == shift.ClientId);
                }
            }

            Employees =  testData.Employees;
        }

        return Employees;
    }
}