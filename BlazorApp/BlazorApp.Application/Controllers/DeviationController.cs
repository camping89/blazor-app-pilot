using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Application.Services;
using BlazorApp.Share.Entities;
using BlazorApp.Share.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviationController : ControllerBase
{
    private readonly DeviationService     _deviationService;
    private readonly IShiftRepository     _shiftRepository;
    private readonly IDeviationRepository _deviationRepository;

    public DeviationController(DeviationService deviationService, IShiftRepository shiftRepository, IDeviationRepository deviationRepository)
    {
        _deviationService    = deviationService;
        _shiftRepository     = shiftRepository;
        _deviationRepository = deviationRepository;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddDeviationRequestInput input)
    {
        var shift                      = await _shiftRepository.Get(input.Deviation.ShiftId.ToString());
        var validateDeviationResultDto = await _deviationService.ValidateDeviation(input.Deviation, shift, false);

        if (validateDeviationResultDto.IsError)
        {
            return BadRequest(validateDeviationResultDto);
        }

        input.Deviation.Id = RandomDataGenerator.GetId();

        await _deviationRepository.Add(input.Deviation);

        return Ok(new ResultDto<Deviation> { Payload = input.Deviation });
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateDeviationRequestInput input)
    {
        var shift                      = await _shiftRepository.Get(input.Deviation.ShiftId.ToString());
        var validateDeviationResultDto = await _deviationService.ValidateDeviation(input.Deviation, shift, true);

        if (validateDeviationResultDto.IsError)
        {
            return BadRequest(validateDeviationResultDto);
        }

        await _deviationRepository.Update(input.Deviation.Id.ToString(), input.Deviation);

        return Ok(new ResultDto<Deviation> { Payload = input.Deviation });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deviation = await _deviationRepository.Get(id);
        if (deviation is not null)
        {
            await _deviationRepository.Delete(deviation.Id.ToString());
        }

        return Ok();
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var deviation = await _deviationRepository.Get(id);
        if (deviation is null)
        {
            return NotFound();
        }
        else
        {
            var shift = await _shiftRepository.Get(deviation.ShiftId.ToString());
            if (shift != null) deviation.Shift = shift;
        }
        return Ok(new ResultDto<Deviation> {Payload = deviation});
    }
}