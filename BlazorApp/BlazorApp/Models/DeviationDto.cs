using System.ComponentModel.DataAnnotations;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;
using BlazorApp.Share.Extensions;

namespace BlazorApp.Models;

public class DeviationDto
{
    public DateTime CreatedAt  { get; set; }
    public DateTime ModifiedAt { get; set; }

    public string EmployeeId { get; set; }
    [Required(ErrorMessage = "Please select shift.")]
    public string ShiftId { get;           set; }
    public DateTime StartTime       { get; set; }
    public DateTime EndTime         { get; set; }
    public int      Duration        => (int)(EndTime - StartTime).TotalMinutes;
    [Required(ErrorMessage = "Please select deviation type.")]
    public string   DeviationTypeId { get; set; }
    [Required(ErrorMessage = "Please enter deviation reason.")]
    public string   Reason          { get; set; }
    [Required(ErrorMessage = "Please select deviation status.")]
    public string   StatusId        { get; set; }
    public int      Id              { get; set; }

    public Deviation ToDeviation()
    {
        return new Deviation
        {
            Id            = Id,
            CreatedAt     = CreatedAt,
            ModifiedAt    = ModifiedAt,
            EmployeeId    = int.Parse(EmployeeId),
            ShiftId       = int.Parse(ShiftId),
            StartTime     = new TimeOnly(StartTime.Hour, StartTime.Minute, StartTime.Second),
            EndTime       = new TimeOnly(EndTime.Hour,   EndTime.Minute,   EndTime.Second),
            DeviationType = DeviationTypeId.ToEnum<DeviationType>(),
            Reason        = Reason,
            Status        = StatusId.ToEnum<DeviationStatus>()
        };
    }
}