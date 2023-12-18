using BlazorApp.Share.Entities;
using BlazorApp.Share.ValueObjects;
using RestSharp;

namespace BlazorApp.Services;

public class ClientApiService : BaseService
{
    public async Task<ResultDto<List<Client>>> Get()
    {
        var request  = new RestRequest("client/get");
        var response = await GetRestClient().ExecuteAsync<ResultDto<List<Client>>>(request);
        return response.Data;
    }
}