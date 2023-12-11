namespace BlazorApp.Share.Models;

public class Client : Entity 
{
    public string Name { get; set; }
    public int NDayparts { get; set; }
    public int NMin { get; set; }

    public int TMin => NDayparts * NMin;
    public IList<Shift> Shifts { get; set; }
}