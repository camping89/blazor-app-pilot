using BlazorApp.Models;
using BlazorApp.Share.Entities;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components;

public partial class ShiftDetailFormComponent
{
    [Parameter] public ShiftDto Shift { get; set; } = new();
    
    private List<KeyValueDto> _shiftStatus;
    private List<Employee>  _employees = new();
    private List<Client>    _clients   = new();

    protected override async Task OnInitializedAsync()
    {
        _employees = (await EmployeeApiService.Get()).Payload;
        _clients   = (await ClientApiService.Get()).Payload;
        
        _shiftStatus = new List<KeyValueDto>
        {
            new()
            {
                Key   = "1",
                Value = "Planned"
            },
            new()
            {
                Key   = "2",
                Value = "Approved"
            },
            new()
            {
                Key   = "3",
                Value = "Completed"
            }
        };
    }
}