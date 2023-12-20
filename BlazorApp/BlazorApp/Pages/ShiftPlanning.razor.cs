using BlazorApp.Common;
using BlazorApp.Models;
using BlazorApp.Share.Entities;

namespace BlazorApp.Pages;

public partial class ShiftPlanning
{
    private List<ShiftPlanningDto> ShiftPlanningDtos { get; set; } = new();
    private bool VisibleProperty { get; set; }

    protected override async Task OnInitializedAsync()
    {
        VisibleProperty   = true;
        ShiftPlanningDtos = new List<ShiftPlanningDto>();
        var employees = (await EmployeeApiService.Get()).Payload;

        foreach (var employee in employees)
        {
            var shiftPlanningDtos = new List<ShiftPlanningDto>();
            var totalDuration = employee.Shifts.Sum(shift => shift.Duration);
            foreach (var shift in employee.Shifts.OrderBy(s => s.Date).ThenBy(s => s.StartTime))
            {
                var shiftPlanningDto = shift.ToShiftPlanningDto(employee.Name);
                shiftPlanningDto.Description = $"Employee: {shiftPlanningDto.EmployeeName}";
                if (shiftPlanningDtos.Any())
                {
                    shiftPlanningDto.ParentId    = shiftPlanningDtos.First().Id;
                    shiftPlanningDto.Description = $"Shift Title: {shiftPlanningDto.Title}";
                    shiftPlanningDtos.Add(shiftPlanningDto);
                    
                    AddDeviationPlanning(shift, employee, shiftPlanningDto, shiftPlanningDtos);
                }
                else
                {
                    shiftPlanningDto.Id *= -1;
                    shiftPlanningDto.DurationDescription = $"Total Shift Duration: {totalDuration} minutes";
                    shiftPlanningDtos.Add(shiftPlanningDto);
                    
                    var subShiftPlanningDto= shift.ToShiftPlanningDto(employee.Name);
                    subShiftPlanningDto.ParentId    = shiftPlanningDtos.First().Id;
                    subShiftPlanningDto.Description = $"Shift Title: {shiftPlanningDto.Title}";
                    shiftPlanningDtos.Add(subShiftPlanningDto);

                    AddDeviationPlanning(shift, employee, subShiftPlanningDto, shiftPlanningDtos);
                }
            }

            ShiftPlanningDtos.AddRange(shiftPlanningDtos);
        }

        VisibleProperty = false;
    }

    private static void AddDeviationPlanning(Shift shift, Employee employee, ShiftPlanningDto shiftPlanningDto,
        List<ShiftPlanningDto> shiftPlanningDtos)
    {
        if (shift.Deviations != null && shift.Deviations.Any())
        {
            var totalDuration = shift.Deviations.Sum(deviation => deviation.Duration);
            foreach (var deviation in shift.Deviations)
            {
                var deviationPlanning = deviation.ToShiftPlanningDto(shift, employee.Name);
                deviationPlanning.ParentId = shiftPlanningDto.Id;
                deviationPlanning.DurationDescription = $"{deviationPlanning.Duration} minutes";
                shiftPlanningDtos.Add(deviationPlanning);
                            
                var subDeviationPlanning = deviation.ToShiftPlanningDto(shift, employee.Name);
                subDeviationPlanning.Id *= -1;
                subDeviationPlanning.StartDate = shiftPlanningDto.StartDate;
                subDeviationPlanning.EndDate = shiftPlanningDto.EndDate;
                subDeviationPlanning.ParentId = shiftPlanningDto.Id;
                subDeviationPlanning.Duration = shiftPlanningDto.Duration;
                subDeviationPlanning.Description = "Deviation Summary";
                subDeviationPlanning.DurationDescription = $"Total Deviation Duration: {totalDuration} minutes";
                shiftPlanningDtos.Add(subDeviationPlanning);
            }
        }
    }

    public async Task ReloadComponent()
    {
        await OnInitializedAsync();
    }
}