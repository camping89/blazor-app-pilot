using RestSharp;

namespace BlazorApp.Services;

public class BaseService
{
    protected RestClient GetRestClient()
    {
        var options = new RestClientOptions("http://localhost:5280");
        var client = new RestClient(options);
        return client;
    }
}