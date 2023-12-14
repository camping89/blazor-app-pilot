namespace BlazorApp.Share.Models;

public class DevationDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string EmployeeId { get; set; }
    public string ShiftId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Duration => (int) (EndTime - StartTime).TotalMinutes;
    public string DevationTypeId { get; set; }
    public string Reason { get; set; }
    public string  StatusId { get; set; }
}