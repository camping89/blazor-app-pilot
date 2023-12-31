using BlazorApp.Share.Enums;

namespace BlazorApp.Share.Entities;

public partial class Shift : Entity
{
    public int         EmployeeId { get; set; }
    public int         ClientId   { get; set; }
    public string      Title      { get; set; }
    public DateOnly    Date       { get; set; }
    public TimeOnly    StartTime  { get; set; }
    public TimeOnly    EndTime    { get; set; }
    public int         Duration   => (int)(EndTime - StartTime).TotalMinutes;
    public ShiftStatus Status     { get; set; }

    public List<Deviation>? Deviations { get; set; } = new();

    public Client? Client { get; set; }

    public Employee? Employee { get; set; }

    public Shift() => Deviations = new List<Deviation>();
}