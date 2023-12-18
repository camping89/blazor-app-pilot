namespace BlazorApp.Share.Extensions;

public static class DateExtensions
{
    public static DateTime ToDateTime(this DateOnly date)
    {
        return new(date.Year, date.Month, date.Day);
    }
    
    public static DateOnly ToDateOnly(this DateTime date)
    {
        return new(date.Year, date.Month, date.Day);
    }
    
    public static TimeOnly ToTimeOnly(this DateTime date)
    {
        return new(date.Hour, date.Minute, date.Second);
    }
    
    public static DateTime ToDateTime(this TimeOnly time, DateTime date)
    {
        return new(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
    }
    
    public static DateTime ToDateTime(this TimeOnly time, DateOnly dateOnly)
    {
        return new(dateOnly.Year, dateOnly.Month, dateOnly.Day, time.Hour, time.Minute, time.Second);
    }
}