using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Share.Entities;
using BlazorApp.Share.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository  _employeeRepository;
    private readonly IDeviationRepository _deviationRepository;
    private readonly IShiftRepository     _shiftRepository;
    private readonly IClientRepository    _clientRepository;

    public EmployeeController
    (IEmployeeRepository  employeeRepository,
     IDeviationRepository deviationRepository,
     IShiftRepository     shiftRepository,
     IClientRepository    clientRepository)
    {
        _employeeRepository  = employeeRepository;
        _deviationRepository = deviationRepository;
        _shiftRepository     = shiftRepository;
        _clientRepository    = clientRepository;
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
                shift.Deviations = new List<Deviation>();
                var deviation = await _deviationRepository.GetByShiftId(shift.Id);
                if (deviation is not null)
                {
                    shift.Deviations.Add(deviation);
                }

                var client = await _clientRepository.Get(shift.ClientId.ToString());
                shift.Client = client;
            }

            employee.Shifts = shifts.ToList();
        }

        var data = new ResultDto<List<Employee>> { Payload = employees };

        return Ok(data);
    }
}