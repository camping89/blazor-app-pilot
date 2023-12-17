using BlazorApp.Application.Repositories;
using BlazorApp.Application.Services;
using BlazorApp.Share.Dtos;
using BlazorApp.Share.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviationController : ControllerBase
{
    private readonly DeviationService _deviationService;
    private readonly IShiftRepository _shiftRepository;
    private readonly IDeviationRepository _deviationRepository;

    public DeviationController(DeviationService deviationService, IShiftRepository shiftRepository, IDeviationRepository deviationRepository)
    {
        _deviationService = deviationService;
        _shiftRepository = shiftRepository;
        _deviationRepository = deviationRepository;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddDeviationRequestInput input)
    {
        var shift = await _shiftRepository.Get(input.Deviation.ShiftId.ToString());
        var validateDeviationResultDto = await _deviationService.ValidateDeviation(input.Deviation, shift, false);
        
        if (validateDeviationResultDto.IsError)
        {
            return BadRequest(validateDeviationResultDto);
        }
        
        input.Deviation.Id = TestDataGenerator.GetId();

        await _deviationRepository.Add(input.Deviation);

        return Ok(new ResultDto<Deviation>
        {
            Data = input.Deviation
        });
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateDeviationRequestInput input)
    {
        var shift = await _shiftRepository.Get(input.Deviation.ShiftId.ToString());
        var validateDeviationResultDto = await _deviationService.ValidateDeviation(input.Deviation, shift, true);
        
        if (validateDeviationResultDto.IsError)
        {
            return BadRequest(validateDeviationResultDto);
        }
        
        await _deviationRepository.Update(input.Deviation.Id.ToString(), input.Deviation);
        
        return Ok(new ResultDto<Deviation>
        {
            Data = input.Deviation
        });
    }
}