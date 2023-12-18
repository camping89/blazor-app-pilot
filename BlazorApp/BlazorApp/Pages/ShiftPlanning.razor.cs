using System.Diagnostics;
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
    private DeviationFormComponent _deviationForm;
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
        switch (args.Item.Id)
        {
            case "toolbarAddShift":
                _shiftForm.Title = "Add Shift";
                _shiftForm.ResetData();
                await _shiftForm.Show();
                break;

            case "toolbarAddDeviation":
                _deviationForm.Title = "Add/Update Deviation";
                _deviationForm.ResetData();
                await _deviationForm.Show();
                break;
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

    public async Task OnCreated(object args)
    {
        await _gantt.ZoomToFitAsync();
    }
    
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

    /// <summary>
    ///    Triggered before the shift form (dialog) is opened.
    /// </summary>
    /// <param name="args"></param>
    public async Task ActionBegin(GanttActionEventArgs<ShiftPlanningDto> args)
    {
        if (args.RequestType == Syncfusion.Blazor.Gantt.Action.BeforeOpenEditDialog)
        {
            args.Cancel = true;
            if (args.RowData.ParentId is not null)
            {
                var shift = await ShiftApiService.Get(args.RowData.Id);
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