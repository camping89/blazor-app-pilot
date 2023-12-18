using System.Drawing;
using BlazorApp.Models;
using BlazorApp.Share.Enums;
using Syncfusion.Blazor.Schedule;

namespace BlazorApp.Components;

public partial class ShiftScheduleComponent
{
    List<ShiftScheduleDto>               DataSource = new();
    private SfSchedule<ShiftScheduleDto> _shiftSchedule;

    protected override async Task OnInitializedAsync()
    {
        var employees = await EmployeeApiService.Get();

        if (employees.Payload != null)
        {
            foreach (var employee in employees.Payload)
            {
                var color = RandomColor();
                foreach (var shift in employee.Shifts)
                {
                    
                    if (shift.Deviations.Any())
                    {
                        var deviationAppointments = new List<ShiftScheduleDto>();
                        foreach (var deviation in shift.Deviations)
                        {
                            deviationAppointments.Add(new ShiftScheduleDto
                            {
                                Id            = shift.Id,
                                Subject       = $"Type: {deviation.DeviationType}",
                                Location      = $"Shift: {shift.Title}",
                                StartTime     = shift.Date.ToDateTime(deviation.StartTime),
                                EndTime       = shift.Date.ToDateTime(deviation.EndTime),
                                Description   = $"Reason: {deviation.Reason}",
                                CategoryColor = color
                            });
                        }
                        
                        DataSource.AddRange(deviationAppointments);

                        var latenessDeviation = shift.Deviations.FirstOrDefault(deviation =>
                            deviation.DeviationType == DeviationType.Lateness);
                        var earlyLeaveDeviation = shift.Deviations.FirstOrDefault(deviation =>
                            deviation.DeviationType == DeviationType.EarlyLeave);

                        DateTime startTime = shift.Date.ToDateTime(shift.StartTime);
                        DateTime endTime = shift.Date.ToDateTime(shift.EndTime);
                        if (latenessDeviation is not null)
                        {
                            startTime = shift.Date.ToDateTime(latenessDeviation.EndTime);
                        }

                        if (earlyLeaveDeviation is not null)
                        {
                            endTime =  shift.Date.ToDateTime(earlyLeaveDeviation.StartTime);
                        }
                        
                        DataSource.Add(new ShiftScheduleDto
                        {
                            Id            = shift.Id,
                            Subject       = $"Shift: {shift.Title}",
                            Location      = $"Client: {shift.Client?.Name ?? ""}",
                            StartTime     = startTime,
                            EndTime       = endTime,
                            Description   = $"Employee: {employee.Name}",
                            CategoryColor = color
                        });
                    }
                    else
                    {
                        DataSource.Add(new ShiftScheduleDto
                        {
                            Id            = shift.Id,
                            Subject       = $"Shift: {shift.Title}",
                            Location      = $"Client: {shift.Client?.Name ?? ""}",
                            StartTime     = shift.Date.ToDateTime(shift.StartTime),
                            EndTime       = shift.Date.ToDateTime(shift.EndTime),
                            Description   = $"Employee: {employee.Name}",
                            CategoryColor = color
                        });
                    }
                }
            }
        }

        await _shiftSchedule.RefreshAsync();
    }

    public void OnEventRendered(EventRenderedArgs<ShiftScheduleDto> args)
    {
        Dictionary<string, object> attributes = new Dictionary<string, object> { { "style", "background:" + args.Data.CategoryColor } };
        args.Attributes = attributes;
    }

    private string RandomColor()
    {
        var randomGen       = new Random();
        var names           = (KnownColor[])Enum.GetValues(typeof(KnownColor));
        var randomColorName = names[randomGen.Next(names.Length)];
        var randomColor     = Color.FromKnownColor(randomColorName);

        return $"#{randomColor.R:X2}{randomColor.G:X2}{randomColor.B:X2}";
    }
}