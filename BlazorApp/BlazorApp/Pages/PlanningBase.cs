using BlazorApp.Common;
using BlazorApp.Models;
using BlazorApp.Share.Entities;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages;

public class PlanningBase : ComponentBase
{
    protected static void AddDeviationPlanning(Shift shift, Employee employee, ShiftPlanningDto shiftPlanningDto,
        List<ShiftPlanningDto> shiftPlanningDtos)
    {
        if (shift.Deviations != null && shift.Deviations.Any())
        {
            var totalDuration = shift.Deviations.Sum(deviation => deviation.Duration);
            var shiftDeviationPlanningDtos = new List<ShiftPlanningDto>();
            foreach (var deviation in shift.Deviations)
            {
                var deviationPlanning = deviation.ToShiftPlanningDto(shift, employee.Name);
                deviationPlanning.ParentId = shiftPlanningDto.Id;
                deviationPlanning.DurationDescription = $"{deviationPlanning.Duration} minutes";
                shiftDeviationPlanningDtos.Add(deviationPlanning);
            }

            var firstDeviation = shift.Deviations.First();
            var subDeviationPlanning = firstDeviation.ToShiftPlanningDto(shift, employee.Name);
            subDeviationPlanning.Id *= -1;
            subDeviationPlanning.StartDate = shiftPlanningDto.StartDate;
            subDeviationPlanning.EndDate = shiftPlanningDto.EndDate;
            subDeviationPlanning.ParentId = shiftPlanningDto.Id;
            subDeviationPlanning.Duration = shiftPlanningDto.Duration;
            subDeviationPlanning.Description = "Deviation Summary";
            subDeviationPlanning.DurationDescription = $"Total Deviation Duration: {totalDuration} minutes";
            shiftDeviationPlanningDtos.Add(subDeviationPlanning);
            
            shiftPlanningDtos.AddRange(shiftDeviationPlanningDtos);
        }
    }
}