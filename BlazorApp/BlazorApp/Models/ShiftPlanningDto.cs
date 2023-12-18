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
    public List<DeviationDto> Deviations         { get; set; } = new();
    public string             EmployeeName      { get; set; }
    public string             ClientName        { get; set; }
    public string             Description       { get; set; }
}