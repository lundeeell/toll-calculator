using TollCalculator.Data;
using TollCalculator.Models;

namespace TollCalculator;

public static class TollCalculator
{
    private const int MaxTollFee = 60;

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public static int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (dates.Length == 0) return 0;

        if (dates.Any(d => d.Date != dates[0].Date))
            throw new ArgumentException("All dates must belong to the same day");

        var intervalStart = dates[0];
        var totalFee = 0;
        var intervalMaxFee = 0;

        foreach (var date in dates)
        {
            var nextFee = GetTollFee(vehicle, date);
            var diff = (date - intervalStart).TotalMinutes;
            const int hourInMinutes = 60;
            
            if (diff < hourInMinutes)
            {
                intervalMaxFee = Math.Max(intervalMaxFee, nextFee);
            }
            else
            {
                totalFee += intervalMaxFee;
                intervalStart = date;
                intervalMaxFee = nextFee;
            }
        }

        totalFee += intervalMaxFee;

        if (totalFee > MaxTollFee) totalFee = MaxTollFee;
        return totalFee;
    }

    private static int GetTollFee(Vehicle vehicle, DateTime date)
    {
        if (TollFeeSchedule.IsTollFeeFreeDate(date) || vehicle.IsTollFeeFree) return 0;

        var timeOfDay = date.TimeOfDay;

        foreach (var (start, end, fee) in TollFeeSchedule.Schedule)
            if (timeOfDay >= start && timeOfDay <= end)
                return fee;

        return 0;
    }
}