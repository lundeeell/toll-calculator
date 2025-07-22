using TollCalculator.Models;

namespace TollCalculator.Data;

public static class TollFeeExcludedVehicles
{
    public static readonly List<VehicleType> GetTollFeeExcludedVehicles =
    [
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military,
    ];
}