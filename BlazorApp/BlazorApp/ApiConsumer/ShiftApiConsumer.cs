using System.Text.Json;
using BlazorApp.Models;
using BlazorApp.Share.Dtos;
using BlazorApp.Share.Entities;
using RestSharp;

namespace BlazorApp.ApiConsumer;

public class ShiftApiConsumer : BaseConsumer
{
    public async Task<ResultDto<Shift>> Add(ShiftDto shiftDto)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            IgnoreNullValues = true
        };
        var json = JsonSerializer.Serialize(new AddShiftRequestInput
        {
            Shift = shiftDto.ToShift()
        }, jsonOptions);
        
        var request =
            new RestRequest("Shift/add", Method.Post).AddParameter("application/json", json,
                ParameterType.RequestBody);
        
        var response = await GetRestClient().ExecuteAsync<ResultDto<Shift>>(request);
        return response.Data;
    }

    public async Task<ResultDto<Shift>> Update(ShiftDto shiftDto)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            IgnoreNullValues = true
        };
        var json = JsonSerializer.Serialize(new UpdateShiftRequestInput
        {
            Shift = shiftDto.ToShift()
        }, jsonOptions);
        
        var request =
            new RestRequest("Shift/update", Method.Post).AddParameter("application/json", json,
                ParameterType.RequestBody);
        var response = await GetRestClient().ExecuteAsync<ResultDto<Shift>>(request);
        
        return response.Data;
    }

    public async Task<Shift> GetById(int id)
    {
        var request =
            new RestRequest($"Shift/get/{id}");
        var response = await GetRestClient().ExecuteAsync<ResultDto<Shift>>(request);
        return response.Data.Data;
    }

    public async Task<List<Shift>> Get()
    {
        var request =
            new RestRequest($"Shift/get");
        var response = await GetRestClient().ExecuteAsync<ResultDto<List<Shift>>>(request);
        return response.Data.Data;
    }
}