namespace BlazorApp.Models;

public class ShiftPlanningDto
{
    public int                Id                { get; set; }
    public string             Title             { get; set; }
    public DateTime           StartDate         { get; set; }
    public DateTime           EndDate           { get; set; }
    public string             Duration          { get; set; }
    public string             DurationUnit      { get; set; }
    public decimal            Progress          { get; set; }
    public int?               ParentId          { get; set; }
    public int                DeviationDuration { get; set; }
    public string             EmployeeName      { get; set; }
    public string             ClientName        { get; set; }
    public string             Description       { get; set; }
    public bool IsDeviation { get; set; }
    public bool HasDeviation { get; set; }
    public string DurationDescription { get; set; }
}