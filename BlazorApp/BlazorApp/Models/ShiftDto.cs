using System.ComponentModel.DataAnnotations;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;
using BlazorApp.Share.Extensions;

namespace BlazorApp.Models;

public class ShiftDto
{
    public int      Id         { get; set; }
    public DateTime CreatedAt  { get; set; }
    public DateTime ModifiedAt { get; set; }
    [Required(ErrorMessage = "Please select {0}.")]
    public string EmployeeId { get; set; }
    [Required(ErrorMessage = "Please select {0}.")]
    public string ClientId { get; set; }

    [Required(ErrorMessage = "Please enter {0}.")]
    public string Title { get;  set; }
    public DateTime Date { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
    public int    Duration => (int)(EndTime - StartTime).TotalMinutes;
    public string StatusId { get; set; }

    public List<DeviationDto> Deviations   { get; set; } = new();
    public string             EmployeeName { get; set; }

    public Shift ToShift()
    {
        var shift = new Shift
        {
            Id         = Id,
            CreatedAt  = CreatedAt,
            ModifiedAt = ModifiedAt,
            EmployeeId = int.Parse(EmployeeId),
            ClientId   = int.Parse(ClientId),
            Title      = Title,
            Date       = new DateOnly(Date.Year, Date.Month, Date.Day),
            StartTime  = new TimeOnly(StartTime.Hour, StartTime.Minute, StartTime.Second),
            EndTime    = new TimeOnly(EndTime.Hour,   EndTime.Minute,   EndTime.Second),
            Status     = StatusId.ToEnum<ShiftStatus>(),
        };

        foreach (var deviation in Deviations)
        {
            if (deviation.DeviationTypeId.ToEnum<DeviationType>() != DeviationType.None)
            {
                deviation.EmployeeId = EmployeeId;
                deviation.ShiftId    = Id.ToString();
                shift.Deviations.Add(deviation.ToDeviation());
            }

        }

        return shift;
    }
}