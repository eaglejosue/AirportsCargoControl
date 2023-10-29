namespace Core.Data.Extensions;

public static class DateTimeExtensions
{
    public static DateTime DateTimeUtc(this DateTime date, bool timeZero = false)
    {
        if (timeZero)
            return new DateTime(
                date.Year,
                date.Month,
                date.Day,
                0, 0, 0, DateTimeKind.Utc);

        return new DateTime(
                date.Year,
                date.Month,
                date.Day,
                23, 59, 59, DateTimeKind.Utc);
    }

    public static DateTime LastHourMinuteAndSecond(this DateTime dateTime) => dateTime.Date.AddDays(1).AddMilliseconds(-1);

    public static int NumberOfTheWeek(this DateTime dateTime) => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);

    public static DateTime FirstDayOfTheWeek(this DateTime dateTime) => FirstDayOfTheWeek(dateTime, CultureInfo.CurrentCulture);

    public static DateTime FirstDayOfTheWeek(DateTime dateTime, CultureInfo cultureInfo)
    {
        DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        DateTime firstDayInWeek = dateTime.Date;
        while (firstDayInWeek.DayOfWeek != firstDay)
            firstDayInWeek = firstDayInWeek.AddDays(-1);
        return firstDayInWeek;
    }

    public static DateTime LastDayOfTheWeek(this DateTime dateTime) => FirstDayOfTheWeek(dateTime, CultureInfo.CurrentCulture).AddDays(6);

    public static DateTime LastDayOfTheWeek(DateTime dateTime, CultureInfo cultureInfo) => FirstDayOfTheWeek(dateTime, cultureInfo).AddDays(6);

    public static DateTime GetEndOfWeek(this DateTime startDate)
    {
        DayOfWeek dayOfWeek = startDate.DayOfWeek;
        int daysOfSunday = DayOfWeek.Sunday - dayOfWeek;

        return startDate.AddDays(daysOfSunday);
    }

    public static DateTime GetEndOfMonth(this DateTime startDate)
    {
        int lastDay = DateTime.DaysInMonth(startDate.Year, startDate.Month);
        return new DateTime(startDate.Year, startDate.Month, lastDay);
    }
}
