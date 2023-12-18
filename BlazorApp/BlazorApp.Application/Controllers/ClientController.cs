using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Share.Entities;
using BlazorApp.Share.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly IBaseRepository<Client> _clientRepository;
    private readonly IShiftRepository _shiftRepository;
    private readonly IDeviationRepository _deviationRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ClientController(IBaseRepository<Client> clientRepository, IShiftRepository shiftRepository, IDeviationRepository deviationRepository, IEmployeeRepository employeeRepository)
    {
        _clientRepository = clientRepository;
        _shiftRepository = shiftRepository;
        _deviationRepository = deviationRepository;
        _employeeRepository = employeeRepository;
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var clients = await _clientRepository.Get();
        
        foreach (var client in clients)
        {
            var shifts = await _shiftRepository.GetByClientId(client.Id);
            foreach (var shift in shifts)
            {
                shift.Deviations = new List<Deviation>();
                var deviation = await _deviationRepository.GetByShiftId(shift.Id);
                if (deviation is not null)
                {
                    shift.Deviations.Add(deviation);
                }

                var employee = await _employeeRepository.Get(shift.EmployeeId.ToString());
                shift.Employee = employee;
            }

            client.Shifts = shifts;
        }
        var data = new ResultDto<List<Client>> { Payload = clients };

        return Ok(data);
    }
}