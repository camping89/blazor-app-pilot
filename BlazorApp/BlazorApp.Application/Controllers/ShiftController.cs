using BlazorApp.Share.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;
[ApiController]
[Route("[controller]")]
public class ShiftController : ControllerBase
{
    [HttpPost("add-shift")]
    public async Task<IActionResult>  Add( Shift shift)
    {
        if (shift.StartTime > shift.EndTime)
        {
            ModelState.AddModelError(nameof(shift.StartTime), "The Start Time should be less than the End Time");
            return BadRequest(ModelState); 
        }
        return Ok(shift);
    }
}