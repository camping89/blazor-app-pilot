namespace BlazorApp.Share.Models;

public class Employee : Entity
{
    public string Name { get; set; }
    public int THoursWeekly { get; set; }
    public int THoursDaily { get; set; }
    public int DaysAvailable { get; set; }
    public int TranzitMin { get; set; }
    public int VacationDays { get; set; }
    
    public List<Shift> Shifts { get; set; }
    public List<Deviation> Deviations { get; set; }
}