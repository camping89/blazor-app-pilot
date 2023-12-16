namespace BlazorApp.Share.Extensions;

public static class StringExtensions
{
    public static T? ToEnum<T>(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return default(T);

        try
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        catch (ArgumentException)
        {
            return default;
        }
    }
}