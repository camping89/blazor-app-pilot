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
}