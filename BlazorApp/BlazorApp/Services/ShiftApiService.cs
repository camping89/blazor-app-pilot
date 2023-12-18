using System.Text.Json;
using BlazorApp.Models;
using BlazorApp.Share.Entities;
using BlazorApp.Share.ValueObjects;
using RestSharp;

namespace BlazorApp.Services;

public class ShiftApiService : BaseService
{
    public async Task<ResultDto<Shift>> Add(ShiftDto shiftDto)
    {
        var jsonOptions = new JsonSerializerOptions { IgnoreNullValues = true };
        var json        = JsonSerializer.Serialize(new AddShiftRequestInput { Shift = shiftDto.ToShift() }, jsonOptions);

        var request = new RestRequest("Shift/add", Method.Post).AddParameter("application/json", json, ParameterType.RequestBody);

        var response = await GetRestClient().ExecuteAsync<ResultDto<Shift>>(request);
        return response.Data;
    }

    public async Task<ResultDto<Shift>> Update(ShiftDto shiftDto)
    {
        var jsonOptions = new JsonSerializerOptions { IgnoreNullValues = true };
        var json        = JsonSerializer.Serialize(new UpdateShiftRequestInput { Shift = shiftDto.ToShift() }, jsonOptions);

        var request  = new RestRequest("Shift/update", Method.Post).AddParameter("application/json", json, ParameterType.RequestBody);
        var response = await GetRestClient().ExecuteAsync<ResultDto<Shift>>(request);

        return response.Data;
    }

    public async Task<Shift> Get(int id)
    {
        var request  = new RestRequest($"Shift/get/{id}");
        var response = await GetRestClient().ExecuteAsync<ResultDto<Shift>>(request);
        return response.Data.Payload;
    }

    public async Task<List<Shift>> Get()
    {
        var request  = new RestRequest($"Shift/get");
        var response = await GetRestClient().ExecuteAsync<ResultDto<List<Shift>>>(request);
        return response.Data.Payload;
    }

    public async Task Delete(string Id)
    {
        var request  = new RestRequest($"Shift/delete/{Id}", Method.Delete);
        await GetRestClient().ExecuteAsync(request);
    }
}