using HseKat.Domain.Abstractions;
using HseKat.Domain.Vehicles;

namespace HseKat.Service.Reports;

public sealed class FleetReportService : IFleetReportService
{
    private readonly IInventoryRepository _repository;
    private readonly IBeginnerPolicy _beginnerPolicy;

    public FleetReportService(IInventoryRepository repository, IBeginnerPolicy beginnerPolicy)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(beginnerPolicy);

        _repository = repository;
        _beginnerPolicy = beginnerPolicy;
    }

    public double GetTotalDailyEnergyKwh()
    {
        return _repository.GetAll()
            .OfType<IEnergyConsumer>()
            .Sum(consumer => consumer.DailyEnergyKwh);
    }

    public IReadOnlyDictionary<string, int> GetCountsByType()
    {
        return _repository.GetAll()
            .GroupBy(item => item.GetType().Name)
            .ToDictionary(group => group.Key, group => group.Count());
    }

    public IReadOnlyList<Vehicle> GetBeginnerVehicles()
    {
        return _repository.GetAll()
            .OfType<Vehicle>()
            .Where(_beginnerPolicy.IsSuitable)
            .OrderBy(vehicle => vehicle.InventoryNumber)
            .ToList();
    }

    public IReadOnlyList<IInventoryItem> GetInventoryList()
    {
        return _repository.GetAll()
            .OrderBy(item => item.InventoryNumber)
            .ToList();
    }
}
