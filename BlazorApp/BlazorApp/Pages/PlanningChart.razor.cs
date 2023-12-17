using BlazorApp.Components;
using BlazorApp.Models;
using BlazorApp.Share.Entities;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace BlazorApp.Pages;

public partial class PlanningChart
{
    private List<TaskDto> TaskCollection { get; set; } = new List<TaskDto>();
    private SfGantt<TaskDto> _gantt;
    private ShiftFormComponent _shiftForm;
    private AddDeviationFormComponent _deviationForm;
    private bool VisibleProperty { get; set; } = false;
    private int _index = 0;
    
    public List<object> ToolbarItems = new List<object> { "CollapseAll", "ExpandAll", "ZoomToFit", 
        new ItemModel() { Text = "Add Shift", TooltipText = "Add Shift", Id = "toolbarAddShift" },
        new ItemModel() { Text = "Add Deviation", TooltipText = "Add Deviation", Id = "toolbarAddDeviation" } };
    
    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "toolbarAddShift")
        {
            _shiftForm.Title = "Add Shift";
            _shiftForm.ResetData();
            await _shiftForm.Show();
        }
        
        if (args.Item.Id == "toolbarAddDeviation")
        {
            _deviationForm.Title = "Add Deviation";
            // _deviationForm.ResetData();
            await _deviationForm.Show();
        }
    }

    private void GanttChartRowInfo(QueryChartRowInfoEventArgs<TaskDto> args)
    {
        if (args.Data.ParentId is null)
        {
            if (_index == 6)
            {
                _index = 1;
            }
            else
            {
                _index = _index + 1;
            }
            args.Row.AddClass(new string[] { $"customize-task-parent-{_index}" });
        }
        else
        {
            args.Row.AddClass(new string[] { $"customize-task-child-{_index}" });
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
                    task.EmployeeName = $"Client: {task.ClientName}" ;
                    tasks.Add(task);
                }
                else
                {
                    task.TaskId *= -1;
                    tasks.Add(task);
                    var subTask =ToTaskDto(shift, employee.Name);
                    subTask.ParentId = tasks.First().TaskId;
                    subTask.EmployeeName = $"Client: {task.ClientName}" ;
                    tasks.Add(subTask);
                }
            }
            TaskCollection.AddRange(tasks);
        }

        this.VisibleProperty = false;
    }
    
    public async Task ActionBegin(GanttActionEventArgs<TaskDto> args)
    {
        if(args.RequestType == Syncfusion.Blazor.Gantt.Action.BeforeOpenEditDialog)
        {
            args.Cancel = true;
            if (args.RowData.ParentId is not null)
            {
                var shift =  await ShiftApiConsumer.GetById(args.RowData.TaskId);
                _shiftForm.ShiftModel = ToShiftDto(shift);
                _shiftForm.Title = "Update Shift";
                _shiftForm.IsDisplayedDeleteButton = true;
                await _shiftForm.Show();
            }
        }
    }

    private async Task OnShiftFromClose()
    {
        await OnInitializedAsync();
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
            Progress = 0,
            DurationUnit = "minute",
            EmployeeName = employeeName
        };

        if (shift.Client is not null)
        {
            taskDto.ClientName = shift.Client.Name;
        }
                
        Console.WriteLine($"shift TaskDto  {taskDto.TaskId}, shift TaskDto Duration {taskDto.Duration}");

        if (shift.Deviations != null && shift.Deviations.Any())
        {
            var deviation = shift.Deviations.First();
            if (deviation is not null)
            {
                var deviationDuration = shift.Deviations.First().Duration;
                taskDto.DeviationDuration = deviationDuration;
                taskDto.Progress = deviationDuration / (decimal)shift.Duration * 100;
            }
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
        if (shift.Deviations != null && shift.Deviations.Any())
        {
            var deviation = shift.Deviations.First();
            return new DeviationDto
            {
                Reason = deviation.Reason,
                EmployeeId = deviation.EmployeeId.ToString(),
                StatusId = ((int) deviation.Status).ToString(),
                StartTime = shift.Date.ToDateTime(deviation.StartTime),
                EndTime = shift.Date.ToDateTime(deviation.EndTime),
                DeviationTypeId = ((int) deviation.DeviationType).ToString(),
                ShiftId = deviation.ShiftId.ToString()
            };
        }

        return new DeviationDto();
    }
}