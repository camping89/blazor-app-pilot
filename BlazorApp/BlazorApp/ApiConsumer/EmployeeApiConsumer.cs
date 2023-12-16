using BlazorApp.Share.Dtos;
using BlazorApp.Share.Entities;
using RestSharp;

namespace BlazorApp.Services;

public class EmployeeApiConsumer : BaseConsumer
{
    public async Task<ResultDto<IList<Employee>>> GetAll()
    {
        var request =
            new RestRequest("Employee/get-all");
        var response = await GetRestClient().ExecuteAsync<ResultDto<IList<Employee>>>(request);
        return response.Data;
    }
}