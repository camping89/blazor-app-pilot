using BlazorApp.Models;
using BlazorApp.Share.Entities;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components;

public partial class ShiftDetailFormComponent
{
    [Parameter]
    public ShiftDto Shift { get; set; } = new()
    {
        Date         = DateTime.Now,
    };
    
    private List<StatusDto> _shiftStatus;
    private List<Employee>  _employees = new();
    private List<Client>    _clients   = new();

    protected override async Task OnInitializedAsync()
    {
        _employees = (await EmployeeApiService.Get()).Payload;
        _clients   = (await ClientApiService.Get()).Payload;
        
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
}