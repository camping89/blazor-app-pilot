using System.ComponentModel.Design.Serialization;

namespace BlazorApp.Share.Entities;

public class Client : Entity 
{
    public string Name { get; set; }
    public int NDayparts { get; set; }
    public int NMin { get; set; }

    public int TMin => NDayparts * NMin;
    public List<Shift> Shifts { get; set; }

    public Client()
    {
        Shifts = new List<Shift>();
    }
}