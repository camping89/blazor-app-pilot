using BlazorApp.Models;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;

namespace BlazorApp.Common;

public static class MapperExtensions
{
    public static ShiftPlanningDto ToShiftPlanningDto(this Deviation deviation, Shift shift, string employeeName)
    {
        var item = new ShiftPlanningDto
        {
            Id           = deviation.Id,
            StartDate    = shift.Date.ToDateTime(deviation.StartTime),
            EndDate      = shift.Date.ToDateTime(deviation.EndTime),
            Duration     = deviation.Duration.ToString(),
            Title        = deviation.Reason,
            Progress     = 0,
            DurationUnit = "minute",
            EmployeeName = employeeName,
            IsDeviation = true,
            Description = $"Deviation Type: {deviation.DeviationType} - Reason: {deviation.Reason}"
        };
        
        if (shift.Client is not null)
        {
            item.ClientName = shift.Client.Name;
        }

        item.Progress = 0;

        return item;
    }
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
            EmployeeName = employeeName,
            DurationDescription = $"{shift.Duration} minutes"
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
        // if (deviation.DeviationType == DeviationType.EarlyLeave)
        // {
        //     item.Progress = (shift.Duration - deviationDuration) / (decimal) shift.Duration * 100;
        // }
        //
        // if (deviation.DeviationType == DeviationType.Lateness)
        // {
        //     item.Progress = deviationDuration / (decimal)shift.Duration * 100;
        // }
        //
        // if (deviation.DeviationType == DeviationType.Illness)
        // {
        //     item.Progress = 100;
        // }

        if (shift.Deviations.Any())
        {
            item.HasDeviation = true;
        }

        item.Progress = 0;
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