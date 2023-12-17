using BlazorApp.Models;
using BlazorApp.Share.Entities;
using Syncfusion.Blazor.Popups;

namespace BlazorApp.Components;

public partial class AddDeviationFormComponent
{
    private   SfDialog DialogObj;
    private DeviationDto _deviationDto = new DeviationDto();
    private IList<Shift>  _shifts = new List<Shift>();
    public   string                      Title      { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _shifts = await ShiftApiConsumer.Get();
    }
        

    private async Task HandleValidSubmit()
    {
        
    }
    
    public async Task Show()
    {
        await this.DialogObj.ShowAsync();
    }
    
}