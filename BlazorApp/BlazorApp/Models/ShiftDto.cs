using System.ComponentModel.DataAnnotations;
using BlazorApp.Share.Enums;
using BlazorApp.Share.Extensions;

namespace BlazorApp.Share.Models;

public class ShiftDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    [Required(ErrorMessage = "Please select {0}.")]
    public string EmployeeId { get; set; }
    [Required(ErrorMessage = "Please select {0}.")]
    public string ClientId { get; set; }
    [Required(ErrorMessage = "Please enter {0}.")]
    public string Title { get; set; }
    public DateTime Date { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    // [Compare("StartTime", ErrorMessage = "The End Time should not equal to Start Time")]
    public DateTime EndTime { get; set; }
    public int Duration => (int) (EndTime - StartTime).TotalMinutes;
    public string  StatusId { get; set; }

    public Shift ToShift()
    {
        return new Shift
        {
            Id = Id,
            CreatedAt = CreatedAt,
            ModifiedAt = ModifiedAt,
            EmployeeId = int.Parse(EmployeeId) ,
            ClientId = int.Parse(ClientId),
            Title = Title,
            Date = new DateOnly(Date.Year, Date.Month, Date.Day),
            StartTime = new TimeOnly(StartTime.Ticks),
            EndTime = new TimeOnly(EndTime.Ticks),
            Status = StatusId.ToEnum<Status>()
        };
    }
}