using RestSharp;

namespace BlazorApp.ApiConsumer;

public class BaseConsumer
{
    protected RestClient GetRestClient()
    {
        var options = new RestClientOptions("http://localhost:5280");
        var client = new RestClient(options);
        return client;
    }
}