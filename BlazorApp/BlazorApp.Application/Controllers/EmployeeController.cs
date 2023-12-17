using BlazorApp.Application.Repositories;
using BlazorApp.Application.Services;
using BlazorApp.Share.Dtos;
using BlazorApp.Share.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDeviationRepository _deviationRepository;
    private readonly IShiftRepository _shiftRepository;
    
    public EmployeeController(IEmployeeRepository employeeRepository, IDeviationRepository deviationRepository, IShiftRepository shiftRepository)
    {
        _employeeRepository = employeeRepository;
        _deviationRepository = deviationRepository;
        _shiftRepository = shiftRepository;
    }
    
    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var employees = await _employeeRepository.Get();
        foreach (var employee in employees)
        {
            var shifts = await _shiftRepository.GetByEmployeeId(employee.Id);
            foreach (var shift in shifts)
            {
                var deviation = await _deviationRepository.GetByShiftId(shift.Id);
                if (deviation is not null)
                {
                    shift.Deviations = new List<Deviation> { deviation };
                }
            }

            employee.Shifts = shifts.ToList();
        }
        var data = new ResultDto<IList<Employee>>
        {
            Data = employees
        };

        return Ok(data);
    }
}