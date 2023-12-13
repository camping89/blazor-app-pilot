using BlazorApp.Application.Services;
using BlazorApp.Share.Models;
using BlazorApp.Share.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly GenerateService _generateService;
    public ClientController(GenerateService generateService)
    {
        _generateService = generateService;
    }
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        var testData = _generateService.GenerateTestData();

        var returnData = new ResultDto<IList<Client>>
        {
            Data = testData.Clients
        };
        return Ok(returnData);
    }
}