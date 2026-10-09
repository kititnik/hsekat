using HseKat.Domain.Vehicles;

namespace HseKat.Service.Inspection;

public interface IServiceCenter
{
    InspectionResult Inspect(Vehicle vehicle);
}
