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

    public ClientController(IBaseRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var data = new ResultDto<List<Client>> { Payload = await _clientRepository.Get() };

        return Ok(data);
    }
}