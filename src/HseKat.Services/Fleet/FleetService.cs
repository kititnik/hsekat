using HseKat.Domain.Abstractions;
using HseKat.Domain.Vehicles;
using HseKat.Service.Inspection;

namespace HseKat.Service.Fleet;

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

    public void RegisterItem(IInventoryItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item is Vehicle)
            throw new ArgumentException("Vehicles must be admitted through inspection.", nameof(item));

        _repository.Add(item);
    }
}
