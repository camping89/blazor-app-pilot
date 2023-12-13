using BlazorApp.Share.Models;
using BlazorApp.Share.Models.Dto;
using RestSharp;

namespace BlazorApp.Consumers;

public class ClientApiConsumer : BaseConsumer
{
    public async Task<ResultDto<IList<Client>>> GetAll()
    {
        var request =
            new RestRequest("Client/get-all");
        var response = await GetRestClient().ExecuteAsync<ResultDto<IList<Client>>>(request);
        return response.Data;
    }
}