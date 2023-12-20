using System.Text.Json;
using BlazorApp.Models;
using BlazorApp.Share.Entities;
using BlazorApp.Share.ValueObjects;
using RestSharp;

namespace BlazorApp.Services;

public class DeviationApiService : BaseService
{
    public async Task<ResultDto<Deviation>> Add(DeviationDto deviationDto)
    {
        var jsonOptions = new JsonSerializerOptions { IgnoreNullValues = true };

        var json = JsonSerializer.Serialize(new AddDeviationRequestInput { Deviation = deviationDto.ToDeviation() }, jsonOptions);

        var request = new RestRequest("deviation/add", Method.Post).AddParameter("application/json", json, ParameterType.RequestBody);

        var response = await GetRestClient().ExecuteAsync<ResultDto<Deviation>>(request);
        return response.Data;
    }

    public async Task<ResultDto<Deviation>> Update(DeviationDto deviationDto)
    {
        var jsonOptions = new JsonSerializerOptions { IgnoreNullValues = true };
        var json        = JsonSerializer.Serialize(new UpdateDeviationRequestInput { Deviation = deviationDto.ToDeviation() }, jsonOptions);

        var request  = new RestRequest("deviation/update", Method.Post).AddParameter("application/json", json, ParameterType.RequestBody);
        var response = await GetRestClient().ExecuteAsync<ResultDto<Deviation>>(request);

        return response.Data;
    }
    
    public async Task Delete(string id)
    {
        var request  = new RestRequest($"deviation/delete/{id}", Method.Delete);
        await GetRestClient().ExecuteAsync(request);
    }

    public async Task<Deviation> Get(string id)
    {
        var request = new RestRequest($"deviation/get/{id}");
        var response = await GetRestClient().ExecuteAsync<ResultDto<Deviation>>(request);
        return response.Data.Payload;
    }
}