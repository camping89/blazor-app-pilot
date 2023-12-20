using BlazorApp.Share.Enums;

namespace BlazorApp.Share.Entities;

public class Deviation : Entity
{
    public int     EmployeeId { get; set; }
    public int     ShiftId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Duration => (int) (EndTime - StartTime).TotalMinutes;
    public DeviationType DeviationType { get; set; }
    public string Reason { get; set; }
    public DeviationStatus  Status { get; set; }
    public Shift? Shift { get; set; }
}