namespace BlazorApp.Models;

public class TaskDto
{
    public int      TaskId            { get; set; }
    public string   TaskName          { get; set; }
    public DateTime StartDate         { get; set; }
    public DateTime EndDate           { get; set; }
    public string   Duration          { get; set; }
    public string   DurationUnit      { get; set; }
    public decimal  Progress          { get; set; }
    public int?     ParentId          { get; set; }
    public int      DeviationDuration { get; set; }
    public string   EmployeeName      { get; set; }
    public string   ClientName        { get; set; }
}