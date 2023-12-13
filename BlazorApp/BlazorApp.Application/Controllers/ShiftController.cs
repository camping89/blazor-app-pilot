using BlazorApp.Share.Models;
using BlazorApp.Share.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;
[ApiController]
[Route("[controller]")]
public class ShiftController : ControllerBase
{
    [HttpPost("add-shift")]
    public async Task<IActionResult>  Add( AddShiftRequestInput input)
    {
        var returnData = new ResultDto<Shift>();
        if (input.Shift.StartTime > input.Shift.EndTime)
        {
            returnData.ErrorDetails = new Dictionary<string, List<string>>
            {
                {nameof(input.Shift.StartTime), new List<string> {"The Start Time should be less than the End Time"}}
            };
            returnData.IsError = true;
            return BadRequest(returnData); 
        }

        returnData.Data = input.Shift;
        return Ok(returnData);
    }
}