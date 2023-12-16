namespace BlazorApp.Models;

public class DeviationDto
{
    public DateTime CreatedAt       { get; set; }
    public DateTime ModifiedAt      { get; set; }
    public string   EmployeeId      { get; set; }
    public string   ShiftId         { get; set; }
    public DateTime StartTime       { get; set; }
    public DateTime EndTime         { get; set; }
    public int      Duration        => (int)(EndTime - StartTime).TotalMinutes;
    public string   DeviationTypeId { get; set; }
    public string   Reason          { get; set; }
    public string   StatusId        { get; set; }
}