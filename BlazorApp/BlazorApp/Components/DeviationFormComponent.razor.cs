using BlazorApp.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components;

public partial class DeviationFormComponent
{
    [Parameter] public DeviationDto      DeviationDto { get; set; } = new();
    private            IList<StatusDto> _status;
    private            IList<StatusDto> _deviationType;

    protected override async Task OnInitializedAsync()
    {
        _status = new List<StatusDto>
        {
            new()
            {
                Id   = 1,
                Name = "Planned"
            },
            new()
            {
                Id   = 2,
                Name = "Approved"
            },
            new()
            {
                Id   = 3,
                Name = "Completed"
            },
            new()
            {
                Id   = 4,
                Name = "Pending"
            },
            new()
            {
                Id   = 5,
                Name = "Rejected"
            }
        };

        _deviationType = new List<StatusDto>
        {
            new()
            {
                Id   = 1,
                Name = "Illness"
            },
            new()
            {
                Id   = 2,
                Name = "Lateness"
            },
            new()
            {
                Id   = 3,
                Name = "EarlyLeave"
            }
        };
    }
}