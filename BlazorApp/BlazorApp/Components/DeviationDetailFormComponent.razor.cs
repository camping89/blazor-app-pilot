using BlazorApp.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components;

public partial class DeviationDetailFormComponent
{
    [Parameter] public DeviationDto DeviationDto { get; set; } = new();

    private List<KeyValueDto> _deviationStatus;
    private List<KeyValueDto> _deviationType;

    protected override async Task OnInitializedAsync()
    {
        _deviationStatus = new List<KeyValueDto>
        {
            new()
            {
                Key   = "1",
                Value = "Pending"
            },
            new()
            {
                Key   = "2",
                Value = "Approved"
            },
            new()
            {
                Key   = "3",
                Value = "Rejected"
            }
        };

        _deviationType = new List<KeyValueDto>
        {
            new()
            {
                Key   = "1",
                Value = "Illness"
            },
            new()
            {
                Key   = "2",
                Value = "Lateness"
            },
            new()
            {
                Key   = "3",
                Value = "EarlyLeave"
            }
        };
    }
}