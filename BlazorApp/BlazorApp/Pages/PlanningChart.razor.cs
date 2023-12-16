using BlazorApp.Components;
using BlazorApp.Models;
using BlazorApp.Share.Entities;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace BlazorApp.Pages;

public partial class PlanningChart
{
    private List<TaskDto> TaskCollection { get; set; } = new List<TaskDto>();
    public SfGantt<TaskDto> Gantt;
    public ShiftFormComponent ShiftForm;
    private bool VisibleProperty { get; set; } = false;
    
    public List<object> ToolbarItems = new List<object> { "CollapseAll", "ExpandAll", "ZoomToFit", new ItemModel() { Text = "Add Shift", TooltipText = "Add Shift", Id = "toolbarFilter" } };
    
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
        this.VisibleProperty = true;
        TaskCollection = new List<TaskDto>();
        var employees = (await EmployeeApiConsumer.GetAll()).Data;

        foreach (var employee in employees)
        {
            var tasks = new List<TaskDto>();
            foreach (var shift in employee.Shifts)
            {
                var task = ToTaskDto(shift, employee.Name);
                if (tasks.Any())
                {
                    task.ParentId = tasks.First().TaskId;
                }
                tasks.Add(task);
            }
            TaskCollection.AddRange(tasks);
        }

        this.VisibleProperty = false;
        // await this.Gantt.RefreshAsync();
    }
    
    public async Task ActionBegin(GanttActionEventArgs<TaskDto> args)
    {
        if(args.RequestType == Syncfusion.Blazor.Gantt.Action.BeforeOpenEditDialog)
        {
            args.Cancel = true;
            var shift =  await ShiftApiConsumer.GetById(args.RowData.TaskId);
            ShiftForm.ShiftModel = ToShiftDto(shift);
            ShiftForm.Title = "Update Shift";
            await ShiftForm.Show();
        }
    }

    private TaskDto ToTaskDto(Shift shift, string employeeName)
    {
        var taskDto = new TaskDto
        {
            TaskId = shift.Id,
            StartDate = shift.Date.ToDateTime(shift.StartTime),
            EndDate = shift.Date.ToDateTime(shift.EndTime),
            Duration = shift.Duration.ToString(),
            TaskName = shift.Title,
            Progress = 100,
            DurationUnit = "minute",
            EmployeeName = employeeName
        };

        if (shift.Client is not null)
        {
            taskDto.ClientName = shift.Client.Name;
        }
                
        Console.WriteLine($"shift TaskDto  {taskDto.TaskId}, shift TaskDto Duration {taskDto.Duration}");

        if (shift.Deviations.Any())
        {
            var deviationDuration = shift.Deviations.First().Duration;
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
            StatusId = ((int) shift.Status).ToString(),
            DeviationDto = ToDeviationDto(shift)
        };
    }

    public DeviationDto ToDeviationDto(Shift shift)
    {
        if (shift.Deviations.Any())
        {
            var Deviation = shift.Deviations.First();
            return new DeviationDto
            {
                Reason = Deviation.Reason,
                EmployeeId = Deviation.EmployeeId.ToString(),
                StatusId = ((int) Deviation.Status).ToString(),
                StartTime = shift.Date.ToDateTime(Deviation.StartTime),
                EndTime = shift.Date.ToDateTime(Deviation.EndTime),
                DeviationTypeId = ((int) Deviation.DeviationType).ToString(),
                ShiftId = Deviation.ShiftId.ToString()
            };
        }

        return new DeviationDto();
    }
}