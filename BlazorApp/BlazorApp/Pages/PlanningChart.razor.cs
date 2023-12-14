using BlazorApp.Components;
using BlazorApp.Share.Models;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace BlazorApp.Pages;

public partial class PlanningChart
{
    private List<TaskDto> TaskCollection { get; set; }
    public SfGantt<TaskDto> Gantt;
    public ShiftFormComponent ShiftForm;
    
    public List<object> ToolbarItems = new List<object> { "ZoomIn", "ZoomOut", "ZoomToFit", new ItemModel() { Text = "Add Shift", TooltipText = "Add Shift", Id = "toolbarFilter" } };
    
    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "toolbarFilter")
        {
            ShiftForm.Title = "Add Shift";
            await ShiftForm.Show();
        }
    }
    
    protected override async Task OnInitializedAsync()
    {
        TaskCollection = new List<TaskDto>();
        var employees = (await EmployeeApiConsumer.GetAll()).Data;

        foreach (var employee in employees)
        {
            foreach (var shift in employee.Shifts)
            {
                TaskCollection.Add(ToTaskDto(shift));
            }
        }

        await this.Gantt.RefreshAsync();
    }
    
    public async Task ActionBegin(GanttActionEventArgs<TaskDto> args)
    {
        if(args.RequestType == Syncfusion.Blazor.Gantt.Action.BeforeOpenEditDialog)
        {
            args.Cancel = true;
            var shift =  await ShiftApiConsumer.GetById(args.RowData.TaskId);
            ShiftForm.ShiftModel = ToShiftDto(shift);
            ShiftForm.DevationModel = ToDevationDto(shift);
            ShiftForm.Title = "Update Shift";
            await ShiftForm.Show();
        }
    }

    private TaskDto ToTaskDto(Shift shift)
    {
        var taskDto = new TaskDto
        {
            TaskId = shift.Id,
            StartDate = shift.Date.ToDateTime(shift.StartTime),
            EndDate = shift.Date.ToDateTime(shift.EndTime),
            Duration = shift.Duration.ToString(),
            TaskName = shift.Title,
            Progress = 100,
            DurationUnit = "minute"
        };
                
        Console.WriteLine($"shift TaskDto  {taskDto.TaskId}, shift TaskDto Duration {taskDto.Duration}");

        if (shift.Devations.Any())
        {
            var deviationDuration = shift.Devations.First().Duration;
            taskDto.DeviationDuration = deviationDuration;
            taskDto.Progress = deviationDuration / (decimal)shift.Duration * 100;
                    
        }

        return taskDto;
    }

    private ShiftDto ToShiftDto(Shift shift)
    {
        return new ShiftDto
        {
            Id = shift.Id,
            CreatedAt = shift.CreatedAt,
            ModifiedAt = shift.ModifiedAt,
            EmployeeId = shift.EmployeeId.ToString(),
            ClientId = shift.ClientId.ToString(),
            Title = shift.Title,
            Date = new DateTime(shift.Date.Year, shift.Date.Month, shift.Date.Day),
            StartTime = shift.Date.ToDateTime(shift.StartTime),
            EndTime = shift.Date.ToDateTime(shift.EndTime),
            StatusId = ((int) shift.Status).ToString()
        };
    }

    public DevationDto ToDevationDto(Shift shift)
    {
        if (shift.Devations.Any())
        {
            var devation = shift.Devations.First();
            return new DevationDto
            {
                Reason = devation.Reason,
                EmployeeId = devation.EmployeeId.ToString(),
                StatusId = ((int) devation.Status).ToString(),
                StartTime = shift.Date.ToDateTime(devation.StartTime),
                EndTime = shift.Date.ToDateTime(devation.EndTime),
                DevationTypeId = ((int) devation.DeviationType).ToString(),
                ShiftId = devation.ShiftId.ToString()
            };
        }

        return new DevationDto();
    }
}