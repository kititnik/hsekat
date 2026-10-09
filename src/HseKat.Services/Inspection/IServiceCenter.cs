using HseKat.Domain.Vehicles;

namespace HseKat.Services.Inspection;

public interface IServiceCenter
{
    InspectionResult Inspect(Vehicle vehicle);
}
