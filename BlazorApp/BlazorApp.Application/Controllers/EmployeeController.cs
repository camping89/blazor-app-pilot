using BlazorApp.Application.Services;
using BlazorApp.Share.Models;
using BlazorApp.Share.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private static IList<Employee> _employees = new List<Employee>();
    private readonly GenerateService _generateService;
    
    public EmployeeController(GenerateService generateService)
    {
        _generateService = generateService;
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        if (!_employees.Any())
        {
            var testData =  _generateService.GenerateTestData();
            foreach (var employee in testData.Employees)
            {
                var shifts = testData.Shifts.Where(shift => shift.EmployeeId == employee.Id).ToList();
                employee.Shifts = shifts;
                foreach (var shift in shifts)
                {
                    shift.Devations = new List<Devation>
                    {
                        testData.Deviations
                            .Last(deviation => deviation.ShiftId == shift.Id && employee.Id == deviation.EmployeeId)
                    };
                    Console.WriteLine(
                        $"shift id {shift.Id}, shift Duration {shift.Duration}, deviation id {shift.Devations.First().Id}, deviation duration {shift.Devations.First().Duration}");
                    shift.Client = testData.Clients.FirstOrDefault(client => client.Id == shift.ClientId);
                }
            }

            _employees =  testData.Employees;
        }

        var returnData = new ResultDto<IList<Employee>>
        {
            Data = _employees
        };
        return Ok(returnData);
    }
}