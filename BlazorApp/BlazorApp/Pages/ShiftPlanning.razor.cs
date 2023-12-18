using BlazorApp.Common;
using BlazorApp.Models;

namespace BlazorApp.Pages;

public partial class ShiftPlanning
{
    
    private List<ShiftPlanningDto>    ShiftPlanningDtos { get; set; } = new();

    private bool                      VisibleProperty { get; set; }
    private int                       _index;
    
    protected override async Task OnInitializedAsync()
    {
        VisibleProperty = true;
        ShiftPlanningDtos      = new List<ShiftPlanningDto>();
        var employees = (await EmployeeApiService.Get()).Payload;

        foreach (var employee in employees)
        {
            var shiftPlanningDtos = new List<ShiftPlanningDto>();
            foreach (var shift in employee.Shifts)
            {
                var shiftPlanningDto = shift.ToShiftPlanningDto(employee.Name);
                shiftPlanningDto.Description = $"Employee: {shiftPlanningDto.EmployeeName}";
                if (shiftPlanningDtos.Any())
                {
                    shiftPlanningDto.ParentId     = shiftPlanningDtos.First().Id;
                    shiftPlanningDto.Description = $"Client: {shiftPlanningDto.ClientName}";
                    shiftPlanningDtos.Add(shiftPlanningDto);
                }
                else
                {
                    shiftPlanningDto.Id *= -1;
                    shiftPlanningDtos.Add(shiftPlanningDto);
                    var subTask = shift.ToShiftPlanningDto(employee.Name);
                    subTask.ParentId     = shiftPlanningDtos.First().Id;
                    subTask.Description = $"Client: {shiftPlanningDto.ClientName}";
                    shiftPlanningDtos.Add(subTask);
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