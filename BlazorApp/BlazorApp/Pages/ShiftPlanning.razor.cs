using BlazorApp.Common;
using BlazorApp.Models;

namespace BlazorApp.Pages;

public partial class ShiftPlanning
{
    private List<ShiftPlanningDto> ShiftPlanningDtos { get; set; } = new();

    private bool VisibleProperty { get; set; }
    private int  _index;

    protected override async Task OnInitializedAsync()
    {
        VisibleProperty   = true;
        ShiftPlanningDtos = new List<ShiftPlanningDto>();
        var employees = (await EmployeeApiService.Get()).Payload;

        foreach (var employee in employees)
        {
            var shiftPlanningDtos = new List<ShiftPlanningDto>();
            foreach (var shift in employee.Shifts.OrderBy(s => s.Date).ThenBy(s => s.StartTime))
            {
                var shiftPlanningDto = shift.ToShiftPlanningDto(employee.Name);
                shiftPlanningDto.Description = $"Employee: {shiftPlanningDto.EmployeeName}";
                if (shiftPlanningDtos.Any())
                {
                    shiftPlanningDto.ParentId    = shiftPlanningDtos.First().Id;
                    shiftPlanningDto.Description = $"Shift Title: {shiftPlanningDto.Title}";
                    shiftPlanningDtos.Add(shiftPlanningDto);
                }
                else
                {
                    shiftPlanningDto.Id *= -1;
                    shiftPlanningDtos.Add(shiftPlanningDto);
                    var sub = shift.ToShiftPlanningDto(employee.Name);
                    sub.ParentId    = shiftPlanningDtos.First().Id;
                    sub.Description = $"Shift Title: {shiftPlanningDto.Title}";
                    shiftPlanningDtos.Add(sub);
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