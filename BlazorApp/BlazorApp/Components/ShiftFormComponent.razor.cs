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

/// <summary>
/// A component to create and edit shifts.
/// </summary>
public partial class ShiftFormComponent
{
    [Parameter]
    public EventCallback OnShiftFormCloseCallback { get; set; }

    [Inject] private ILogger<ShiftFormComponent> Logger { get; set; }
    public           string                      Title  { get; set; }

    protected CustomFormValidator customFormValidator;

    private SfDialog Dialog;

    
    public  bool            IsDisplayedDeleteButton = false;

    public ShiftDto Shift { get; set; } = new()
    {
        Date         = DateTime.Now,
    };

    public bool EnabledDropBox { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        
    }

    private async Task HandleValidSubmit()
    {
        customFormValidator.ClearFormErrors();
        try
        {
            var resultData = Shift.Id == 0 ? await ShiftApiService.Add(Shift) : await ShiftApiService.Update(Shift);
            if (resultData.IsError)
            {
                customFormValidator.DisplayFormErrors(resultData.ErrorDetails);
                throw new HttpRequestException("Validation failed.");
            }

            Shift.EmployeeName = resultData.Payload.Employee.Name;

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
        await ShiftApiService.Delete(Shift.Id.ToString());
        await Hide();
    }

    public async Task Hide()
    {
        await Dialog.HideAsync();
        await OnShiftFormCloseCallback.InvokeAsync();
        IsDisplayedDeleteButton = false;
    }

    public async Task Show()
    {
        await Dialog.ShowAsync();
    }

    public void ResetData()
    {
        Shift = new ShiftDto
        {
            Date         = DateTime.Now,
            Deviations = new List<DeviationDto>()
        };
    }
}