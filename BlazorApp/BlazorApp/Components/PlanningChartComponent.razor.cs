using BlazorApp.Common;
using BlazorApp.Models;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace BlazorApp.Components;

public partial class PlanningChartComponent
{
    [Parameter]
    public List<ShiftPlanningDto>    ShiftPlanningDtos { get; set; } = new();
    [Parameter]
    public string ColumnHeaderText { get; set; }
    [Parameter]
    public EventCallback ReloadComponent { get; set; }
    
    private SfGantt<ShiftPlanningDto> _gantt;
    private ShiftFormComponent        _shiftForm;
    private DeviationFormComponent _deviationForm;
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
        },
        new ItemModel
        {
            Text = "Column Chooser", 
            TooltipText = "Column Chooser", 
            Id = "toolbarColumnChooser"
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
                _deviationForm.Title = "Add Deviation";
                _deviationForm.ResetData();
                await _deviationForm.Show();
                break;
            
            case "toolbarColumnChooser":
                await _gantt.OpenColumnChooser(100, 50);
                break;
        }
    }
    
    private void GanttChartRowInfo(QueryChartRowInfoEventArgs<ShiftPlanningDto> args)
    {
        if (args.Data.IsDeviation && args.Data.Id < 0)
        {
            args.Row.AddClass(new []{"hide-taskbar"});
        }
        else
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
                args.Row.AddClass(new []{"hide-taskbar"});
            }
            else
            {
                if (args.Data.HasDeviation)
                {
                    args.Row.AddClass(new[] { $"customize-task-parent-{_index}" });
                }
                else
                {
                    if (args.Data.IsDeviation)
                    {
                        args.Row.AddClass(new[] { $"customize-task-child-{_index}-sub" });
                    }
                    else
                    {
                        args.Row.AddClass(new[] { $"customize-task-child-{_index}" });
                    }
                    
                }
                
            }
        }
    }
    
    private void Callback(RowDataBoundEventArgs<ShiftPlanningDto> obj)
    {
        if (obj.Data.ParentId is null)
        {
            obj.Row.AddStyle(new []{"font-weight: bold"});
        }
    }
    
    public async Task OnCreated(object args)
    {
        // await _gantt.ZoomToFitAsync();
    }
    
    protected override async Task OnInitializedAsync()
    {
        
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
                if (args.RowData.IsDeviation)
                {
                    if (args.RowData.Id > 0)
                    {
                        var deviation = await DeviationApiService.Get(args.RowData.Id.ToString());
                        _deviationForm.DeviationDto = deviation.ToDeviationDto(deviation.Shift);
                        _deviationForm.Title = "Update Deviation";
                        _deviationForm.IsDisplayedDeleteButton = true;
                        _deviationForm.EnabledShitDropBox = false;
                        await _deviationForm.Show();
                    }
                }
                else
                {
                    var shift = await ShiftApiService.Get(args.RowData.Id);
                    _shiftForm.Shift                   = shift.ToShiftDto();
                    _shiftForm.Title                   = "Update Shift";
                    _shiftForm.IsDisplayedDeleteButton = true;
                    _shiftForm.EnabledDropBox = false;
                    await _shiftForm.Show();
                }
            }
        }
    }

    private async Task OnShiftFormClose()
    {
        await ReloadComponent.InvokeAsync();
    }

    private async Task OnAddUpdateDeviationFormClose()
    {
        await ReloadComponent.InvokeAsync();
    }
}