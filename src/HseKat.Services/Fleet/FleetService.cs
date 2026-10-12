using HseKat.Domain.Abstractions;
using HseKat.Domain.Things;
using HseKat.Domain.Vehicles;
using HseKat.Services.Inspection;

namespace HseKat.Services.Fleet;

public sealed class FleetService : IFleetService
{
    private readonly IServiceCenter _serviceCenter;
    private readonly IInventoryRepository _repository;

    public FleetService(IServiceCenter serviceCenter, IInventoryRepository repository)
    {
        ArgumentNullException.ThrowIfNull(serviceCenter);
        ArgumentNullException.ThrowIfNull(repository);

        _serviceCenter = serviceCenter;
        _repository = repository;
    }

    public InspectionResult AdmitVehicle(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        var result = _serviceCenter.Inspect(vehicle);
        if (result == InspectionResult.Accepted)
            _repository.Add(vehicle);

        return result;
    }

    public void RegisterItem(Thing item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _repository.Add(item);
    }
}
