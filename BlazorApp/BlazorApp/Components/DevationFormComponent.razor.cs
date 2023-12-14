using BlazorApp.Components.Base;
using BlazorApp.Share.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components;

public partial class DevationFormComponent
{
    [Parameter]
    public DevationDto DevationDto { get; set; } = new DevationDto();
    private IList<StatusDto> _status;
    private IList<StatusDto> _devationType;

    protected override async Task OnInitializedAsync()
    {
        _status = new List<StatusDto>
        {
            new()
            {
                Id = 1,
                Name = "Planned"
            },
            new StatusDto
            {
                Id = 2,
                Name = "Approved"
            },
            new StatusDto
            {
                Id = 3,
                Name = "Completed"
            },
            new StatusDto
            {
                Id = 4,
                Name = "Pending"
            },
            new StatusDto
            {
                Id = 5,
                Name = "Rejected"
            }
        };

        _devationType = new List<StatusDto>
        {
            new StatusDto
            {
                Id = 1,
                Name = "Illness"
            },
            new StatusDto
            {
                Id = 2,
                Name = "Lateness"
            },
            new StatusDto
            {
                Id = 3,
                Name = "EarlyLeave"
            }
        };
    }
}