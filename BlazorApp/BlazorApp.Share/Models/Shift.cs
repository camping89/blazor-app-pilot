using BlazorApp.Share.Enums;

namespace BlazorApp.Share.Models;

public class Shift : EntityBase
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public int EmployeeId { get; set; }
    public int ClientId { get; set; }
    public string Title { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Duration => (int) (EndTime - StartTime).TotalMinutes;
    public Status Status { get; set; }
}