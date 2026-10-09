using HseKat.Domain.Vehicles;

namespace HseKat.Services.Inspection;

public sealed class BasicServiceCenter : IServiceCenter
{
    public InspectionResult Inspect(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        return vehicle.State == TechnicalState.Good
            ? InspectionResult.Accepted
            : InspectionResult.Rejected;
    }
}
