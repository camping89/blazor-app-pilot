using BlazorApp.Share.Entities;
using BlazorApp.Share.ValueObjects;
using RestSharp;

namespace BlazorApp.Services;

public class EmployeeApiService : BaseService
{
    public async Task<ResultDto<List<Employee>>> Get()
    {
        var request  = new RestRequest("employee/get");
        var response = await GetRestClient().ExecuteAsync<ResultDto<List<Employee>>>(request);
        return response.Data;
    }
}