using BlazorApp.Models;
using BlazorApp.Share.Entities;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Popups;

namespace BlazorApp.Components;

public partial class AddDeviationFormComponent
{
    [Parameter]
    public EventCallback OnAddUpdateDeviationFormCloseCallback { get; set; }

    [Inject] ILogger<AddDeviationFormComponent> Logger { get; set; }
    private  SfDialog                           DialogObj;
    private  DeviationDto                       _deviationDto = new();
    private  List<Shift>                       _shifts       = new List<Shift>();

    protected CustomFormValidator customFormValidator;

    public string Title { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _shifts = await ShiftApiService.Get();
    }

    private async Task HandleValidSubmit()
    {
        customFormValidator.ClearFormErrors();
        try
        {
            var resultData = _deviationDto.Id == 0 ? await DeviationApiService.Add(_deviationDto) : await DeviationApiService.Update(_deviationDto);
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

    private async Task OnCancel()
    {
        await Hide();
    }

    public async Task Hide()
    {
        await this.DialogObj.HideAsync();
        await OnAddUpdateDeviationFormCloseCallback.InvokeAsync();
    }

    public async Task Show()
    {
        await this.DialogObj.ShowAsync();
    }

    public void OnValueChange(ChangeEventArgs<string, Shift> args)
    {
        var shift = _shifts.FirstOrDefault(_ => _.Id == args.ItemData.Id);
        if (shift?.Deviations != null && shift.Deviations.Any() && shift.Deviations.All(deviation => deviation != null))
        {
            var deviation = shift.Deviations.First();
            _deviationDto = new DeviationDto
            {
                Id = deviation.Id,
                Reason = deviation.Reason,
                EmployeeId = deviation.EmployeeId.ToString(),
                StatusId = ((int) deviation.Status).ToString(),
                StartTime = shift.Date.ToDateTime(deviation.StartTime),
                EndTime = shift.Date.ToDateTime(deviation.EndTime),
                DeviationTypeId = ((int) deviation.DeviationType).ToString(),
                ShiftId = deviation.ShiftId.ToString()
            };
        }
        else
        {
            _deviationDto = new DeviationDto
            {
                ShiftId    = shift.Id.ToString(),
                EmployeeId = shift.EmployeeId.ToString()
            };
        }
    }

    public void ResetData()
    {
        _deviationDto = new DeviationDto();
    }
}