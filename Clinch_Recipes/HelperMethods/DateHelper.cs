namespace Clinch_Recipes.HelperMethods;

public static class DateHelper
{
    public static string ToCustomDateString(this DateTime date)
    {
        string daySuffix = GetDaySuffix(date.Day);
        string formattedDate = $"{date:dd}{daySuffix} {date:MMM}";

        if (date.Year != DateTime.Now.Year)
        {
            formattedDate += date.ToString(", yyyy");
        }

        string formattedTime = date.ToString("hh:mm tt");
        return $"{formattedDate} at {formattedTime}";
    }

    private static string GetDaySuffix(int day)
    {
        return day switch
        {
            1 or 21 or 31 => "st",
            2 or 22 => "nd",
            3 or 23 => "rd",
            _ => "th",
        };
    }
}
