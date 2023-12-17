using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Application.Services;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;
using BlazorApp.Share.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class ShiftController : ControllerBase
{
    private readonly IShiftRepository     _shiftRepository;
    private readonly IEmployeeRepository  _employeeRepository;
    private readonly IClientRepository    _clientRepository;
    private readonly IDeviationRepository _deviationRepository;
    private readonly DeviationService     _deviationService;

    public ShiftController
    (IShiftRepository     shiftRepository,
     IEmployeeRepository  employeeRepository,
     IClientRepository    clientRepository,
     IDeviationRepository deviationRepository,
     DeviationService     deviationService)
    {
        _shiftRepository     = shiftRepository;
        _employeeRepository  = employeeRepository;
        _clientRepository    = clientRepository;
        _deviationRepository = deviationRepository;
        _deviationService    = deviationService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddShiftRequestInput input)
    {
        var       returnData = await ValidateShift(input.Shift, false);
        Deviation deviation  = null;
        if (!returnData.IsError)
        {
            if (input.Shift.Deviations != null && input.Shift.Deviations.Any())
            {
                deviation = input.Shift.Deviations.First();
                var validateDeviationResultDto = await _deviationService.ValidateDeviation(deviation, input.Shift, false);
                if (validateDeviationResultDto.IsError)
                {
                    returnData.ErrorDetails = validateDeviationResultDto.ErrorDetails;
                    returnData.IsError      = validateDeviationResultDto.IsError;
                    return BadRequest(returnData);
                }
            }
        }

        if (returnData.IsError)
        {
            return BadRequest(returnData);
        }

        input.Shift.Id = TestDataGenerator.GetId();
        await _shiftRepository.Add(input.Shift);

        if (deviation is not null && _deviationService.HasDeviation(deviation))
        {
            deviation.Id         = TestDataGenerator.GetId();
            deviation.ShiftId    = input.Shift.Id;
            deviation.EmployeeId = input.Shift.EmployeeId;

            await _deviationRepository.Add(deviation);
            input.Shift.Deviations = new List<Deviation> { deviation };
        }

        var employee = await _employeeRepository.Get(input.Shift.EmployeeId.ToString());
        input.Shift.Employee = employee;

        returnData.Payload = input.Shift;

        return Ok(returnData);
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var shifts = await _shiftRepository.Get();
        foreach (var shift in shifts)
        {
            var client    = await _clientRepository.Get(shift.ClientId.ToString());
            var employee  = await _employeeRepository.Get(shift.EmployeeId.ToString());
            var deviation = await _deviationRepository.GetByShiftId(shift.Id);
            shift.Client     = client;
            shift.Employee   = employee;
            shift.Deviations = new List<Deviation> { deviation };
        }

        return Ok(new ResultDto<List<Shift>> { Payload = shifts });
    }

    [HttpGet("get/{Id}")]
    public async Task<IActionResult> Get(string id)
    {
        var shift      = await _shiftRepository.Get(id);
        var returnData = new ResultDto<Shift> { Payload = shift };
        return Ok(returnData);
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateShiftRequestInput input)
    {
        var       returnData = await ValidateShift(input.Shift, true);
        Deviation deviation  = null;

        if (!returnData.IsError)
        {
            if (input.Shift.Deviations != null && input.Shift.Deviations.Any())
            {
                deviation = input.Shift.Deviations.First();
                var validateDeviationResultDto = await _deviationService.ValidateDeviation(deviation, input.Shift, true);
                if (validateDeviationResultDto.IsError)
                {
                    returnData.ErrorDetails = validateDeviationResultDto.ErrorDetails;
                    returnData.IsError      = validateDeviationResultDto.IsError;
                    return BadRequest(returnData);
                }
            }
        }

        if (returnData.IsError)
        {
            return BadRequest(returnData);
        }

        var employee = await _employeeRepository.Get(input.Shift.EmployeeId.ToString());
        await _shiftRepository.Update(input.Shift.Id.ToString(), input.Shift);
        input.Shift.Employee = employee;

        if (deviation is not null && _deviationService.HasDeviation(deviation))
        {
            await _deviationRepository.Update(deviation.Id.ToString(), deviation);
            input.Shift.Deviations = new List<Deviation> { deviation };
        }
        else
        {
            var existingDeviation = await _deviationRepository.GetByShiftId(input.Shift.Id);
            if (existingDeviation is not null)
            {
                await _deviationRepository.Delete(existingDeviation.Id.ToString());
            }
        }

        returnData.Payload = input.Shift;

        return Ok(returnData);
    }

    private async Task<ResultDto<Shift>> ValidateShift(Shift shift, bool isUpdate)
    {
        var returnData = new ResultDto<Shift> { ErrorDetails = new Dictionary<string, List<string>>() };
        if (isUpdate)
        {
            var existingShift = await _shiftRepository.Get(shift.Id.ToString());
            if (existingShift is null)
            {
                returnData.ErrorDetails.Add(nameof(shift.Id), new List<string> { "The ShiftId is not existing" });
            }
        }

        if (shift.StartTime >= shift.EndTime)
        {
            returnData.ErrorDetails.Add(nameof(shift.StartTime), new List<string> { "The Start Time should be less than the End Time" });
        }

        var employee = await _employeeRepository.Get(shift.EmployeeId.ToString());
        if (employee is null)
        {
            returnData.ErrorDetails.Add(nameof(shift.EmployeeId), new List<string> { "The EmployeeId is not existing" });
        }

        var client = await _clientRepository.Get(shift.ClientId.ToString());
        if (client is null)
        {
            returnData.ErrorDetails.Add(nameof(shift.ClientId), new List<string> { "The ClientId is not existing" });
        }

        if (string.IsNullOrWhiteSpace(shift.Title))
        {
            returnData.ErrorDetails.Add(nameof(shift.Title), new List<string> { "The Title should not be null or empty" });
        }

        if (shift.Status == ShiftStatus.None)
        {
            returnData.ErrorDetails.Add(nameof(shift.Status), new List<string> { "The Status is invalid" });
        }

        if (returnData.ErrorDetails.Any())
        {
            returnData.IsError = true;
        }

        if (shift.Date < new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day))
        {
            returnData.ErrorDetails.Add(nameof(shift.Date), new List<string> { "The Date is invalid" });
        }

        return returnData;
    }
}