using PublicHoliday;

namespace TollCalculator.Data;

public static class TollFeeSchedule
{
    public static readonly List<(TimeSpan Start, TimeSpan End, int Fee)> Schedule =
    [
        (new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 59), 8), // 06:00-06:29
        (new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 59), 13), // 06:30-06:59
        (new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 59), 18), // 07:00-07:59
        (new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 59), 13), // 08:00-08:29
        (new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 59), 8), // 08:30-14:59
        (new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 59), 13), // 15:00-15:29
        (new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 59), 18), // 15:30-16:59
        (new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 59), 13), // 17:00-17:59
        (new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 59), 8), // 18:00-18:29
    ];

    public static bool IsTollFeeFreeDate(DateTime date) =>
        date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
        || date.Month == 7
        || new SwedenPublicHoliday().IsPublicHoliday(date);
}
