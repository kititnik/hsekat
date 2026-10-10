using HseKat.Domain.Things;
using HseKat.Domain.Vehicles;
using HseKat.Repository;
using HseKat.Services.Reports;
using NSubstitute;

namespace HseKat.Tests;

public class FleetReportServiceTests
{
    private readonly InMemoryInventoryRepository _repository = new();
    private readonly FleetReportService _reportService;

    public FleetReportServiceTests()
    {
        _reportService = new FleetReportService(_repository, new MinEaseBeginnerPolicy(6));
    }

    [Fact]
    public void GetTotalDailyEnergyKwh_AllEnergyConsumerTypes_ShouldSumAll()
    {
        _repository.Add(new ElectricScooter(1, "Test", TechnicalState.Good, 7, 0.5));
        _repository.Add(new ElectricBicycle(2, "Test", TechnicalState.Good, 5, 1.5));
        _repository.Add(new Bicycle(3, "Test", TechnicalState.Good, 8));
        _repository.Add(new DockStation(4, "Test", 2.0));
        _repository.Add(new ChargingCabinet(5, "Test", 3.0));
        
        var result = _reportService.GetTotalDailyEnergyKwh();

        Assert.Equal(7.0, result);
    }

    [Fact]
    public void GetTotalDailyEnergyKwh_EmptyFleet_ShouldReturnZero()
    {
        var result = _reportService.GetTotalDailyEnergyKwh();

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCountsByType_MixedFleet_ShouldCountEachType()
    {
        _repository.Add(new Helmet(1, "Helmet"));
        _repository.Add(new Helmet(2, "Helmet2"));
        _repository.Add(new ElectricScooter(3, "Test", TechnicalState.Good, 7, 0.5));

        var result = _reportService.GetCountsByType();

        Assert.Equal(2, result.Count);
        Assert.Equal(2, result[nameof(Helmet)]);
        Assert.Equal(1, result[nameof(ElectricScooter)]);
    }

    [Theory]
    [InlineData(5, false)]
    [InlineData(6, true)]
    public void GetBeginnerVehicles_EaseBoundary_ShouldReturnExpectedResult(int ease, bool expectedIncluded)
    {
        var vehicle = new ElectricScooter(1, "Test", TechnicalState.Good, ease, 0.5);
        _repository.Add(vehicle);

        var result = _reportService.GetBeginnerVehicles();

        Assert.Equal(expectedIncluded, result.Contains(vehicle));
    }

    [Fact]
    public void GetBeginnerVehicles_MixedFleet_ShouldReturnOnlySuitableVehicles()
    {
        var bicycle = new Bicycle(1, "Test", TechnicalState.Good, 8);
        _repository.Add(bicycle);
        _repository.Add(new ElectricScooter(2, "Test", TechnicalState.Good, 3, 0.5));
        _repository.Add(new Helmet(3, "Helmet"));
        _repository.Add(new DockStation(4, "Dock", 2.0));

        var result = _reportService.GetBeginnerVehicles();

        Assert.Same(bicycle, Assert.Single(result));
    }

    [Fact]
    public void GetInventoryList_AddedOutOfOrder_ShouldSortByInventoryNumber()
    {
        _repository.Add(new Helmet(3, "Helmet"));
        _repository.Add(new Bicycle(1, "Test", TechnicalState.Good, 8));
        _repository.Add(new DockStation(2, "Dock", 2.0));

        var result = _reportService.GetInventoryList();

        Assert.Equal([1, 2, 3], result.Select(item => item.InventoryNumber));
    }
}
