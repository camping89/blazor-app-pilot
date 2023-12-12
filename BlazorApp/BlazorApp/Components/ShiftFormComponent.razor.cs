using System.Net;
using System.Text.Json;
using BlazorApp.Components.Base;
using BlazorApp.Share.Enums;
using BlazorApp.Share.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace BlazorApp.Components;

public partial class  ShiftFormComponent
{
    // [Parameter]
    public int ShiftId { get; set; }
    
    public string Title { get; set; }
    
    private ShiftDto shiftModel = new ShiftDto
    {
        Date = DateTime.Now
    };

    private SfDialog DialogObj;

    private IList<Employee> _employees = new List<Employee>();
    private IList<Client> _clients = new List<Client>();
    private IList<StatusDto> _status;
    [Inject]
    HttpClient Http { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _employees = await EmployService.GetEmployees();
        _clients = await ClientService.GetClients();
        _status = new List<StatusDto>
        {
            new()
            {
                Id = 1,
                Name = "Planned"
            },
            new StatusDto
            {
                Id = 2,
                Name = "Approved"
            },
            new StatusDto
            {
                Id = 3,
                Name = "Completed"
            },
            new StatusDto
            {
                Id = 4,
                Name = "Pending"
            },
            new StatusDto
            {
                Id = 5,
                Name = "Rejected"
            }
        };
    }
    
    protected CustomFormValidator customFormValidator;
    [Inject]
    ILogger<ShiftFormComponent> Logger { get; set; }
    protected bool isAddingSuccess = false;
    
    private async Task HandleValidSubmit()
    {
        customFormValidator.ClearFormErrors();
        isAddingSuccess = false;
        try
        {
            var jsonOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };
            var json = JsonSerializer.Serialize(shiftModel.ToShift(), jsonOptions);
            
            var options = new RestClientOptions("http://localhost:5280");
            var client = new RestClient(options);
            var a = JsonConvert.SerializeObject(shiftModel.ToShift());
            // var request = new RestRequest("Shift/add-shift").AddJsonBody(shiftModel.ToShift());
            var request =
                new RestRequest("Shift/add-shift", Method.Post).AddParameter("application/json", json,
                    ParameterType.RequestBody);
            
            var response = await client.ExecuteAsync<Dictionary<string, List<string>>>(request);
            var errors = response.Data;
            Console.WriteLine("Test");
            
            // var a = JsonConvert.SerializeObject(shiftModel);
            // var response = await Http.PostAsJsonAsync("http://localhost:5280/Shift/add-shift", shiftModel.ToShift());
            // var errors = await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();

            if (response.StatusCode == HttpStatusCode.BadRequest && errors.Count > 0)
            {
                customFormValidator.DisplayFormErrors(errors);
                throw new HttpRequestException($"Validation failed. Status Code: {response.StatusCode}");
            }
            else
            {
                isAddingSuccess = true;
                Logger.LogInformation("The registration is successful");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message);
        }
    }

    private async Task OnClick()
    {
        // await Hide();
    }
    
    private async Task OnCancel()
    {
        await Hide();
    }

    public async Task Hide()
    {
        await this.DialogObj.HideAsync();
    }

    public async Task Show()
    {
        await this.DialogObj.ShowAsync();
    }
}