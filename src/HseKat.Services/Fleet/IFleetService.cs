using HseKat.Domain.Abstractions;
using HseKat.Domain.Vehicles;
using HseKat.Services.Inspection;

namespace HseKat.Services.Fleet;

public interface IFleetService
{
    InspectionResult AdmitVehicle(Vehicle vehicle);
    void RegisterItem(IInventoryItem item);
}
