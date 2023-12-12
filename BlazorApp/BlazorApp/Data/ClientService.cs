using BlazorApp.Share.Models;

namespace BlazorApp.Data;

public class ClientService
{
    private readonly GenerateService _generateService;
    public ClientService(GenerateService generateService)
    {
        _generateService = generateService;
    }

    public async Task<IList<Client>> GetClients()
    {
        var testData = _generateService.GenerateTestData();
        return testData.Clients;
    }
}