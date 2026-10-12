using HseKat.Domain.Things;
using HseKat.Domain.Vehicles;
using HseKat.Services.Inspection;

namespace HseKat.Services.Fleet;

public interface IFleetService
{
    InspectionResult AdmitVehicle(Vehicle vehicle);
    void RegisterItem(Thing item);
}
