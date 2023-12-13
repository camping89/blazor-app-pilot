using System.Text.Json;
using BlazorApp.Share.Models;
using BlazorApp.Share.Models.Dto;
using RestSharp;

namespace BlazorApp.Consumers;

public class ShiftApiConsumer : BaseConsumer
{
    public async Task<ResultDto<Shift>> AddShift(ShiftDto shiftDto)
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
            new RestRequest("Shift/add-shift", Method.Post).AddParameter("application/json", json,
                ParameterType.RequestBody);
        
        var response = await GetRestClient().ExecuteAsync<ResultDto<Shift>>(request);
        return response.Data;
    }
}