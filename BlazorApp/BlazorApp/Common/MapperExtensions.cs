using BlazorApp.Models;
using BlazorApp.Share.Entities;

namespace BlazorApp.Common;

public static class MapperExtensions
{
    public static ShiftPlanningDto ToShiftPlanningDto(this Shift shift, string employeeName)
    {
        var item = new ShiftPlanningDto
        {
            Id           = shift.Id,
            StartDate    = shift.Date.ToDateTime(shift.StartTime),
            EndDate      = shift.Date.ToDateTime(shift.EndTime),
            Duration     = shift.Duration.ToString(),
            Title        = shift.Title,
            Progress     = 0,
            DurationUnit = "minute",
            EmployeeName = employeeName
        };

        if (shift.Client is not null)
        {
            item.ClientName = shift.Client.Name;
        }

        Console.WriteLine($"shift ShiftPlanningDto  {item.Id}, shift ShiftPlanningDto Duration {item.Duration}");

        if (shift.Deviations == null || !shift.Deviations.Any()) return item;

        // note: for now, shift has only one deviation
        var deviation = shift.Deviations.FirstOrDefault();
        if (deviation is null) return item;

        var deviationDuration = shift.Deviations.First().Duration;
        item.DeviationDuration = deviationDuration;
        item.Progress          = deviationDuration / (decimal)shift.Duration * 100;

        foreach (var shiftDeviation in shift.Deviations)
        {
            item.Deviations.Add(shiftDeviation.ToDeviationDto(shift));
        }

        return item;
    }

    public static ShiftDto ToShiftDto(this Shift shift) => new()
    {
        Id           = shift.Id,
        CreatedAt    = shift.CreatedAt,
        ModifiedAt   = shift.ModifiedAt,
        EmployeeId   = shift.EmployeeId.ToString(),
        ClientId     = shift.ClientId.ToString(),
        Title        = shift.Title,
        Date         = new DateTime(shift.Date.Year, shift.Date.Month, shift.Date.Day),
        StartTime    = shift.Date.ToDateTime(shift.StartTime),
        EndTime      = shift.Date.ToDateTime(shift.EndTime),
        StatusId     = ((int)shift.Status).ToString(),
        Deviations = shift.Deviations.Select(deviation => deviation.ToDeviationDto(shift)).ToList()
    };

    public static DeviationDto ToDeviationDto(this Deviation deviation, Shift shift)
    {
        return new DeviationDto
        {
            Reason          = deviation.Reason,
            EmployeeId      = deviation.EmployeeId.ToString(),
            StatusId        = ((int)deviation.Status).ToString(),
            StartTime       = shift.Date.ToDateTime(deviation.StartTime),
            EndTime         =  shift.Date.ToDateTime(deviation.EndTime),
            DeviationTypeId = ((int)deviation.DeviationType).ToString(),
            ShiftId         = deviation.ShiftId.ToString()
        };
    }
}