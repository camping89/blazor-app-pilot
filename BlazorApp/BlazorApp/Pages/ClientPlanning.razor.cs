using BlazorApp.Common;
using BlazorApp.Models;

namespace BlazorApp.Pages;

public partial class ClientPlanning
{
    private List<ShiftPlanningDto>    ShiftPlanningDtos { get; set; } = new();
    
    private bool                      VisibleProperty { get; set; }
    private int                       _index;
    
    protected override async Task OnInitializedAsync()
    {
        VisibleProperty = true;
        ShiftPlanningDtos      = new List<ShiftPlanningDto>();
        var clients = (await ClientApiService.Get()).Payload;

       foreach (var client in clients)
        {
            var shiftPlanningDtos = new List<ShiftPlanningDto>();
            foreach (var shift in client.Shifts)
            {
                var shiftPlanningDto = shift.ToShiftPlanningDto(client.Name);
                shiftPlanningDto.Description = $"Client: {client.Name}";
                if (shiftPlanningDtos.Any())
                {
                    shiftPlanningDto.ParentId     = shiftPlanningDtos.First().Id;
                    shiftPlanningDto.Description = $"Employee: {shiftPlanningDto.EmployeeName}";
                    shiftPlanningDtos.Add(shiftPlanningDto);
                }
                else
                {
                    shiftPlanningDto.Id *= -1;
                    shiftPlanningDtos.Add(shiftPlanningDto);
                    var subPlanningDto = shift.ToShiftPlanningDto(client.Name);
                    subPlanningDto.ParentId     = shiftPlanningDtos.First().Id;
                    subPlanningDto.Description = $"Employee: {shiftPlanningDto.EmployeeName}";
                    shiftPlanningDtos.Add(subPlanningDto);
                }
            }
            ShiftPlanningDtos.AddRange(shiftPlanningDtos);
        }

        VisibleProperty = false;
    }
    
    public async Task ReloadComponent()
    {
        await OnInitializedAsync();
    }
}