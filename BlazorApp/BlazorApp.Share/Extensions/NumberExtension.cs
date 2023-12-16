using System.Globalization;

namespace BlazorApp.Share.Extensions;

public static class NumberExtension
{
    public static T? ToEnum<T>(this int? value) => value.HasValue ? ToEnum<T>(value.Value) : default;

    public static T? ToEnum<T>(this int value)
    {
        if (Enum.IsDefined(typeof(T), value))
        {
            return (T)Enum.Parse(typeof(T), value.ToString(CultureInfo.InvariantCulture));
        }
        return default;
    }
}