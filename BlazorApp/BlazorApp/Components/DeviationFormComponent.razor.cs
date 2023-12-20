using BlazorApp.Models;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Extensions;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Popups;

namespace BlazorApp.Components;

public partial class DeviationFormComponent
{
    [Parameter]
    public EventCallback OnAddUpdateDeviationFormCloseCallback { get; set; }

    [Inject] ILogger<DeviationFormComponent> Logger { get; set; }
    private  SfDialog                        DialogObj;
    private  List<Shift>                     _shifts                 = new();
    public   bool                            IsDisplayedDeleteButton = false;

    protected CustomFormValidator customFormValidator;

    public string Title { get; set; }
    public  DeviationDto                    DeviationDto           = new();
    public bool EnabledShitDropBox = true;

    protected override async Task OnInitializedAsync()
    {
        _shifts = await ShiftApiService.Get();
    }

    private async Task HandleValidSubmit()
    {
        customFormValidator.ClearFormErrors();
        try
        {
            var resultData = DeviationDto.Id == 0 ? await DeviationApiService.Add(DeviationDto) : await DeviationApiService.Update(DeviationDto);
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

    private async Task OnDelete()
    {
        await DeviationApiService.Delete(DeviationDto.Id.ToString());
        await Hide();
    }

    private async Task OnCancel()
    {
        await Hide();
    }

    public async Task Hide()
    {
        IsDisplayedDeleteButton = false;
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
        DeviationDto = new DeviationDto
        {
            ShiftId    = shift.Id.ToString(),
            EmployeeId = shift.EmployeeId.ToString(),
            StartTime = shift.StartTime.ToDateTime(shift.Date),
            EndTime = shift.EndTime.ToDateTime(shift.Date),
        };
    }

    public void ResetData()
    {
        DeviationDto = new DeviationDto();
    }
}