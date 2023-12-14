using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BlazorApp.Share.Enums;
using BlazorApp.Share.Extensions;

namespace BlazorApp.Share.Models;

public class Shift : Entity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public int EmployeeId { get; set; }
    public int ClientId { get; set; }
    public string Title { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int Duration => (int) (EndTime - StartTime).TotalMinutes;
    public Status Status { get; set; }

    public List<Devation>? Devations { get; set; }

    public Client? Client { get; set; }

    public Employee? Employee { get; set; }
}