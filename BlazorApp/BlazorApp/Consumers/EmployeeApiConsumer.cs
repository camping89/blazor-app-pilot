using BlazorApp.Share.Models;
using BlazorApp.Share.Models.Dto;
using RestSharp;

namespace BlazorApp.Consumers;

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