using BlazorApp.Common;
using BlazorApp.Components;
using BlazorApp.Models;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Navigations;

namespace BlazorApp.Pages;

public partial class ShiftPlanning
{
    private SfGantt<ShiftPlanningDto> _gantt;
    private List<ShiftPlanningDto>    ShiftPlanningDtos { get; set; } = new();
    private ShiftFormComponent        _shiftForm;
    private AddDeviationFormComponent _deviationForm;
    private bool                      VisibleProperty { get; set; }
    private int                       _index;

    public readonly List<object> ToolbarItems = new()
    {
        "CollapseAll",
        "ExpandAll",
        "ZoomToFit",
        "Search",
        new ItemModel
        {
            Text        = "Add Shift",
            TooltipText = "Add Shift",
            Id          = "toolbarAddShift"
        },
        new ItemModel
        {
            Text        = "Add Deviation",
            TooltipText = "Add Deviation",
            Id          = "toolbarAddDeviation"
        }
    };

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
            _deviationForm.Title = "Add/Update Deviation";
            _deviationForm.ResetData();
            await _deviationForm.Show();
        }
    }

    private void GanttChartRowInfo(QueryChartRowInfoEventArgs<ShiftPlanningDto> args)
    {
        if (args.Data.ParentId is null)
        {
            if (_index == 6)
            {
                _index = 1;
            }
            else
            {
                _index += 1;
            }

            args.Row.AddClass(new[] { $"customize-task-parent-{_index}" });
        }
        else
        {
            args.Row.AddClass(new[] { $"customize-task-child-{_index}" });
        }
    }

    protected override async Task OnInitializedAsync()
    {
        VisibleProperty = true;
        ShiftPlanningDtos      = new List<ShiftPlanningDto>();
        var employees = (await EmployeeApiService.Get()).Payload;

        foreach (var employee in employees)
        {
            var tasks = new List<ShiftPlanningDto>();
            foreach (var shift in employee.Shifts)
            {
                var task = shift.ToShiftPlanningDto(employee.Name);
                if (tasks.Any())
                {
                    task.ParentId     = tasks.First().Id;
                    task.EmployeeName = $"Client: {task.ClientName}";
                    tasks.Add(task);
                }
                else
                {
                    task.Id *= -1;
                    tasks.Add(task);
                    var subTask = shift.ToShiftPlanningDto(employee.Name);
                    subTask.ParentId     = tasks.First().Id;
                    subTask.EmployeeName = $"Client: {task.ClientName}";
                    tasks.Add(subTask);
                }
            }

            ShiftPlanningDtos.AddRange(tasks);
        }

        VisibleProperty = false;
    }

    public async Task ActionBegin(GanttActionEventArgs<ShiftPlanningDto> args)
    {
        if (args.RequestType == Syncfusion.Blazor.Gantt.Action.BeforeOpenEditDialog)
        {
            args.Cancel = true;
            if (args.RowData.ParentId is not null)
            {
                var shift = await ShiftApiService.GetById(args.RowData.Id);
                _shiftForm.Shift                   = shift.ToShiftDto();
                _shiftForm.Title                   = "Update Shift";
                _shiftForm.IsDisplayedDeleteButton = true;
                await _shiftForm.Show();
            }
        }
    }

    private async Task OnShiftFormClose()
    {
        await OnInitializedAsync();
    }

    private async Task OnAddUpdateDeviationFormClose()
    {
        await OnInitializedAsync();
    }
}