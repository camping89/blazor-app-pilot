using BlazorApp.Share.Dtos;
using BlazorApp.Share.Entities;
using RestSharp;

namespace BlazorApp.ApiConsumer;

public class ClientApiConsumer : BaseConsumer
{
    public async Task<ResultDto<IList<Client>>> GetAll()
    {
        var request =
            new RestRequest("Client/get");
        var response = await GetRestClient().ExecuteAsync<ResultDto<IList<Client>>>(request);
        return response.Data;
    }
}