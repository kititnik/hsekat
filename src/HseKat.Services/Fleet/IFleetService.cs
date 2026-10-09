using HseKat.Domain.Abstractions;
using HseKat.Domain.Vehicles;
using HseKat.Service.Inspection;

namespace HseKat.Service.Fleet;

public interface IFleetService
{
    InspectionResult AdmitVehicle(Vehicle vehicle);
    void RegisterItem(IInventoryItem item);
}
