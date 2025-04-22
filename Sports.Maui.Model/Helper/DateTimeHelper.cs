namespace Sports.Maui.Model.Helper;

using HtmlAgilityPack;

public static class DateTimeHelper
{
    public static DateTime? GetTimeFromNode(HtmlNode htmlNode, bool timeAlreadyLocal)
    {
        try
        {
            var gameTimeString = htmlNode.InnerText;
            var style = timeAlreadyLocal ? System.Globalization.DateTimeStyles.AssumeLocal : System.Globalization.DateTimeStyles.AssumeUniversal;
            var result = DateTime.ParseExact(gameTimeString, "HH:mm", null, style);
            var localDate = style == System.Globalization.DateTimeStyles.AssumeLocal ? result : result.ToLocalTime();
            return localDate;
        }
        catch
        {
            return null;
        }
    }

    public static string? GetGameDateDisplay(DateTime? dateTime)
    {
        return dateTime?.ToString("ddd MMM d");
    }

    public static string? GameTimeDisplay(DateTime? dateTime)
    {
        return dateTime?.ToString("h:mm tt");
    }

    public static string WeekStartEndDisplay(DateTime start, DateTime end)
    {
        var startDisplay = start.ToString("MMM d");
        var endDisplay = end.ToString("MMM d");
        return $"{startDisplay} - {endDisplay}";
    }
}
