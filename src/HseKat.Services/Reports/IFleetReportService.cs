using HseKat.Domain.Abstractions;
using HseKat.Domain.Vehicles;

namespace HseKat.Services.Reports;

public interface IFleetReportService
{
    double GetTotalDailyEnergyKwh();
    IReadOnlyDictionary<string, int> GetCountsByType();
    IReadOnlyList<Vehicle> GetBeginnerVehicles();
    IReadOnlyList<IInventoryItem> GetInventoryList();
}
