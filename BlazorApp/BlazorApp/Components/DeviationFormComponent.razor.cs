using BlazorApp.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components;

public partial class DeviationFormComponent
{
    [Parameter] public DeviationDto      DeviationDto { get; set; } = new();
    private            IList<StatusDto> _deviationStatus;
    private            IList<StatusDto> _deviationType;

    protected override async Task OnInitializedAsync()
    {
        _deviationStatus = new List<StatusDto> { new() { Id = "1", Name = "Pending" }, new() { Id = "2", Name = "Approved" }, new() { Id = "3", Name = "Rejected" } };

        _deviationType = new List<StatusDto>
        {
            new()
            {
                Id   = "1",
                Name = "Illness"
            },
            new()
            {
                Id   = "2",
                Name = "Lateness"
            },
            new()
            {
                Id   = "3",
                Name = "EarlyLeave"
            }
        };
    }
}