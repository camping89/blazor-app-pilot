using System.Net;
using System.Text.Json;
using BlazorApp.Components.Base;
using BlazorApp.Consumers;
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
    public string Title { get; set; }

    public ShiftDto ShiftModel { get; set; } = new ShiftDto
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
        _employees = (await EmployeeApiConsumer.GetAll()).Data;
        _clients = (await ClientApiConsumer.GetAll()).Data;
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

            var resultData = await ShiftApiConsumer.AddShift(ShiftModel);

            if (resultData.IsError)
            {
                customFormValidator.DisplayFormErrors(resultData.ErrorDetails);
                throw new HttpRequestException("Validation failed.");
            }

            await Hide();
            Logger.LogInformation("The registration is successful");
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