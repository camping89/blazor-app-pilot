namespace BlazorApp.Share.Entities;

public class Employee : Entity
{
    public string Name { get; set; }
    public int THoursWeekly { get; set; }
    public int THoursDaily { get; set; }
    public int DaysAvailable { get; set; }
    public int TranzitMin { get; set; }
    public int VacationDays { get; set; }
    
    public List<Shift> Shifts { get; set; }

    public Employee()
    {
        Shifts = new List<Shift>();
    }
}