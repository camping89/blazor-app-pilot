using System.Net;
using System.Text.Json;
using BlazorApp.Models;
using BlazorApp.Pages;
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
    [Inject] ILogger<ShiftFormComponent> Logger     { get; set; }
    public   string                      Title      { get; set; }
    
    private   SfDialog DialogObj;

    private IList<Employee>  _employees = new List<Employee>();
    private IList<Client>    _clients   = new List<Client>();
    private IList<StatusDto> _shiftStatus;
    private IList<StatusDto> _deviationStatus;
    public bool IsDisplayedDeleteButton = false;
    
    [Parameter]
    public EventCallback OnShiftFormCloseCallback { get; set; }

    public ShiftDto ShiftModel { get; set; } = new()
    {
        Date         = DateTime.Now,
        DeviationDto = new DeviationDto()
    };

    protected override async Task OnInitializedAsync()
    {
        _employees = (await EmployeeApiConsumer.GetAll()).Data;
        _clients   = (await ClientApiConsumer.GetAll()).Data;
        _shiftStatus = new List<StatusDto>
        {
            new()
            {
                Id   = "1",
                Name = "Planned"
            },
            new()
            {
                Id   = "2",
                Name = "Approved"
            },
            new()
            {
                Id   = "3",
                Name = "Completed"
            }
        };

        
    }

    protected CustomFormValidator customFormValidator;

    private async Task HandleValidSubmit()
    {
        customFormValidator.ClearFormErrors();
        try
        {
            var resultData = ShiftModel.Id == 0 ? await ShiftApiConsumer.Add(ShiftModel) : await ShiftApiConsumer.Update(ShiftModel);
            if (resultData.IsError)
            {
                customFormValidator.DisplayFormErrors(resultData.ErrorDetails);
                throw new HttpRequestException("Validation failed.");
            }

            ShiftModel.EmployeeName = resultData.Data.Employee.Name;
            
            await Hide();
            Logger.LogInformation("The registration is successful");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message);
        }
    }

    private async Task OnCancel()
    {
        await Hide();
    }

    private async Task OnDelete()
    {
        await Hide();
    }

    public async Task Hide()
    {
        await this.DialogObj.HideAsync();
        await OnShiftFormCloseCallback.InvokeAsync();
        IsDisplayedDeleteButton = false;
    }

    public async Task Show()
    {
        await this.DialogObj.ShowAsync();
    }

    public void ResetData()
    {
        ShiftModel = new ShiftDto { Date = DateTime.Now, DeviationDto = new DeviationDto() };
    }
}