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

        var request = new RestRequest("Deviation/add", Method.Post).AddParameter("application/json", json, ParameterType.RequestBody);

        var response = await GetRestClient().ExecuteAsync<ResultDto<Deviation>>(request);
        return response.Data;
    }

    public async Task<ResultDto<Deviation>> Update(DeviationDto deviationDto)
    {
        var jsonOptions = new JsonSerializerOptions { IgnoreNullValues = true };
        var json        = JsonSerializer.Serialize(new UpdateDeviationRequestInput { Deviation = deviationDto.ToDeviation() }, jsonOptions);

        var request  = new RestRequest("Deviation/update", Method.Post).AddParameter("application/json", json, ParameterType.RequestBody);
        var response = await GetRestClient().ExecuteAsync<ResultDto<Deviation>>(request);

        return response.Data;
    }
    
    public async Task Delete(string Id)
    {
        var request  = new RestRequest($"Deviation/delete/{Id}", Method.Delete);
        await GetRestClient().ExecuteAsync(request);
    }
}