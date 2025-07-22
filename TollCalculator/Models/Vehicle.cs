using TollCalculator.Data;

namespace TollCalculator.Models;

public record Vehicle(VehicleType Type)
{
    public bool IsTollFeeFree => TollFeeExcludedVehicles.GetTollFeeExcludedVehicles.Contains(Type);
}