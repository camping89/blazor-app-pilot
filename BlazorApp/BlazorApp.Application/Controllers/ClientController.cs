using BlazorApp.Application.Repositories;
using BlazorApp.Application.Services;
using BlazorApp.Share.Dtos;
using BlazorApp.Share.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly GenerateService _generateService;
    
    private readonly IBaseRepository<Client> _clientRepository;
    
    public ClientController(GenerateService generateService, IBaseRepository<Client> clientRepository)
    {
        _generateService       = generateService;
        _clientRepository = clientRepository;
    }

    [HttpGet("get")]
    public async Task<List<Client>> Get() => await _clientRepository.Get();

    // [HttpGet("get-all")]
    // public async Task<IActionResult> Get()
    // {
    //     var testData = _generateService.GenerateTestData();
    //
    //     var returnData = new ResultDto<IList<Client>>
    //     {
    //         Data = testData.Clients
    //     };
    //     return Ok(returnData);
    // }
}