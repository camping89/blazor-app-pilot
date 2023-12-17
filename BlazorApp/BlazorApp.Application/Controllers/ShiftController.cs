using BlazorApp.Application.Repositories;
using BlazorApp.Application.Services;
using BlazorApp.Share.Dtos;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class ShiftController : ControllerBase
{
    private readonly IShiftRepository _shiftRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IDeviationRepository _deviationRepository;

    public ShiftController(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository, IClientRepository clientRepository, IDeviationRepository deviationRepository)
    {
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
        _clientRepository = clientRepository;
        _deviationRepository = deviationRepository;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddShiftRequestInput input)
    {
        var returnData = await ValidateShift(input.Shift, false);
        Deviation deviation = null;
        if (!returnData.IsError)
        {
            if (input.Shift.Deviations != null && input.Shift.Deviations.Any())
            {
                deviation = input.Shift.Deviations.First();
                returnData = await ValidateDeviation(deviation, input.Shift, false);
            }
        }
        
        if (returnData.IsError)
        {
            return BadRequest(returnData);
        }

        input.Shift.Id = TestDataGenerator.GetId();
        await _shiftRepository.Add(input.Shift);

        if (deviation is not null && HasDeviation(deviation))
        {
            deviation.Id = TestDataGenerator.GetId();
            deviation.ShiftId = input.Shift.Id;
            deviation.EmployeeId = input.Shift.EmployeeId;
            

            await _deviationRepository.Add(deviation);
            input.Shift.Deviations = new List<Deviation> { deviation };
        }

        var employee = await _employeeRepository.Get(input.Shift.EmployeeId.ToString());
        input.Shift.Employee = employee;
        
        returnData.Data = input.Shift;
        
        return Ok(returnData);
    }
    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var shifts = await _shiftRepository.Get();
        foreach (var shift in shifts)
        {
            var client = await _clientRepository.Get(shift.ClientId.ToString());
            var employee = await _employeeRepository.Get(shift.EmployeeId.ToString());
            var deviation = await _deviationRepository.GetByShiftId(shift.Id);
            shift.Client = client;
            shift.Employee = employee;
            shift.Deviations = new List<Deviation> {deviation};
        }

        return Ok(new ResultDto<List<Shift>>
        {
            Data = shifts
        });

    }

    [HttpGet("get/{Id}")]
    public async Task<IActionResult> Get(string id)
    {
        var shift = await _shiftRepository.Get(id);
        var returnData = new ResultDto<Shift> { Data = shift };
        return Ok(returnData);
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateShiftRequestInput input)
    {
        var returnData = await ValidateShift(input.Shift, true);
        Deviation deviation = null;
        
        if (!returnData.IsError)
        {
            if (input.Shift.Deviations != null && input.Shift.Deviations.Any())
            {
                deviation = input.Shift.Deviations.First();
                returnData = await ValidateDeviation(deviation, input.Shift, true);
            }
        }
        
        if (returnData.IsError)
        {
            return BadRequest(returnData);
        }
        
        var employee = await _employeeRepository.Get(input.Shift.EmployeeId.ToString());
        await _shiftRepository.Update(input.Shift.Id.ToString(), input.Shift);
        input.Shift.Employee = employee;

        if (deviation is not null && HasDeviation(deviation))
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

        returnData.Data = input.Shift;
        
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
            returnData.ErrorDetails.Add(nameof(shift.EmployeeId), new List<string>{"The EmployeeId is not existing"});
        }

        var client = await _clientRepository.Get(shift.ClientId.ToString());
        if (client is null)
        {
            returnData.ErrorDetails.Add(nameof(shift.ClientId), new List<string>{"The ClientId is not existing"});
        }

        if (string.IsNullOrWhiteSpace(shift.Title))
        {
            returnData.ErrorDetails.Add(nameof(shift.Title), new List<string>{"The Title should not be null or empty"});
        }

        if (shift.Status == ShiftStatus.None)
        {
            returnData.ErrorDetails.Add(nameof(shift.Status), new List<string>{"The Status is invalid"});
        }

        if (returnData.ErrorDetails.Any())
        {
            returnData.IsError = true;
        }

        if (shift.Date < new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day))
        {
            returnData.ErrorDetails.Add(nameof(shift.Date), new List<string>{"The Date is invalid"});
        }

        return returnData;
    }

    private async Task<ResultDto<Shift>> ValidateDeviation(Deviation deviation, Shift shift,  bool isUpdate)
    {
        if (HasDeviation(deviation))
        {
            var returnData = new ResultDto<Shift> { ErrorDetails = new Dictionary<string, List<string>>() };
            if (isUpdate && deviation.Id != 0)
            {
                var existingDeviation = await _deviationRepository.Get(deviation.Id.ToString());
                if (existingDeviation is null)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.Id), new List<string>{"The DeviationId is not existing"});
                }
            }
            
            if (deviation.StartTime >= deviation.EndTime)
            {
                returnData.ErrorDetails.Add(nameof(deviation.StartTime), new List<string> { "The Start Time should be less than the End Time" });
            }

            if (string.IsNullOrWhiteSpace(deviation.Reason))
            {
                returnData.ErrorDetails.Add(nameof(deviation.Reason), new List<string> { "The Reason should not be null or empty" });
            }

            if (deviation.Status == DeviationStatus.None)
            {
                returnData.ErrorDetails.Add(nameof(deviation.Status), new List<string> { "The Status is invalid" });
            }

            ValidateDeviationTime(deviation, shift, returnData);
            

            if (returnData.ErrorDetails.Any())
            {
                returnData.IsError = true;
            }

            return returnData;
        }

        return new ResultDto<Shift>();
    }

    private static void ValidateDeviationTime(Deviation deviation, Shift shift, ResultDto<Shift> returnData)
    {
        switch (deviation.DeviationType)
        {
            case DeviationType.Illness:
                if (deviation.StartTime != shift.StartTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.StartTime), new List<string> { "The Deviation StartTime is invalid" });
                }

                if (deviation.EndTime != shift.EndTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.EndTime), new List<string> { "The Deviation EndTime is invalid" });
                }
                break;
            case DeviationType.Lateness:
                if (deviation.EndTime != shift.EndTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.EndTime), new List<string> { "The Deviation EndTime is invalid" });
                }

                if (deviation.StartTime < shift.StartTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.StartTime), new List<string> { "The Deviation StartTime is invalid" });
                }
                break;
            case DeviationType.EarlyLeave:
                if (deviation.StartTime != shift.StartTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.StartTime), new List<string> { "The Deviation StartTime is invalid" });
                }

                if (deviation.EndTime > shift.EndTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.EndTime), new List<string> { "The Deviation EndTime is invalid" });
                }
                break;
        }
    }

    private bool HasDeviation(Deviation deviation)
    {
        return deviation.DeviationType != DeviationType.None;
    }
}