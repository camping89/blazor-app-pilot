using System.Net;
using System.Text.Json;
using BlazorApp.Models;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BlazorApp.Components;

public partial class ShiftFormComponent
{
    [Inject] HttpClient                  HttpClient { get; set; }
    [Inject] ILogger<ShiftFormComponent> Logger     { get; set; }
    public   string                      Title      { get; set; }

    protected bool     IsAddingSuccess = false;
    private   SfDialog DialogObj;

    private IList<Employee>  _employees = new List<Employee>();
    private IList<Client>    _clients   = new List<Client>();
    private IList<StatusDto> _status;

    public ShiftDto ShiftModel { get; set; } = new()
    {
        Date         = DateTime.Now,
        DeviationDto = new DeviationDto()
    };

    protected override async Task OnInitializedAsync()
    {
        _employees = (await EmployeeApiConsumer.GetAll()).Data;
        _clients   = (await ClientApiConsumer.GetAll()).Data;
        _status = new List<StatusDto>
        {
            new()
            {
                Id   = 1,
                Name = "Planned"
            },
            new()
            {
                Id   = 2,
                Name = "Approved"
            },
            new()
            {
                Id   = 3,
                Name = "Completed"
            },
            new()
            {
                Id   = 4,
                Name = "Pending"
            },
            new()
            {
                Id   = 5,
                Name = "Rejected"
            }
        };
    }

    protected CustomFormValidator customFormValidator;

    private async Task HandleValidSubmit()
    {
        customFormValidator.ClearFormErrors();
        IsAddingSuccess = false;
        try
        {
            var resultData = ShiftModel.Id == 0 ? await ShiftApiConsumer.AddShift(ShiftModel) : await ShiftApiConsumer.UpdateShift(ShiftModel);
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