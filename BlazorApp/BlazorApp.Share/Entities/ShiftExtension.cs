namespace BlazorApp.Share.Entities;

public partial class Shift
{
    public string ShortTitle
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Title)) return string.Empty;
            string shortTitle;
            if (Title.Length > 50)
            {
                shortTitle = Title.Substring(0, 30);
                shortTitle = $"{shortTitle} ... - {StartTime} - {EndTime}";
            }
            else
            {
                shortTitle = $"{Title} - {StartTime} - {EndTime}";
            }

            return shortTitle;

        }
    }
}