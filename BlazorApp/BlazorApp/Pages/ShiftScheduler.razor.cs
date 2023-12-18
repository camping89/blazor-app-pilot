using System.Drawing;
using BlazorApp.Models;
using Syncfusion.Blazor.Schedule;

namespace BlazorApp.Pages;

public partial class ShiftScheduler
{
    List<ShiftSchedulerDto>               DataSource = new();
    private SfSchedule<ShiftSchedulerDto> _shiftScheduler;

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
                    DataSource.Add(new ShiftSchedulerDto
                    {
                        Id            = shift.Id,
                        Subject       = $"Shift: {shift.Title}",
                        Location      = $"Client: {shift.Client?.Name ?? ""}",
                        StartTime     = shift.Date.ToDateTime(shift.StartTime),
                        EndTime       = shift.Date.ToDateTime(shift.EndTime),
                        Description   = $"Note: {shift.Title}",
                        CategoryColor = color
                    });
                }
            }
        }

        await _shiftScheduler.RefreshAsync();
    }

    public void OnEventRendered(EventRenderedArgs<ShiftSchedulerDto> args)
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