using BlazorApp.Share.Dtos;
using BlazorApp.Share.Entities;
using RestSharp;

namespace BlazorApp.ApiConsumer;

public class EmployeeApiConsumer : BaseConsumer
{
    public async Task<ResultDto<IList<Employee>>> GetAll()
    {
        var request =
            new RestRequest("Employee/get");
        var response = await GetRestClient().ExecuteAsync<ResultDto<IList<Employee>>>(request);
        return response.Data;
    }
}