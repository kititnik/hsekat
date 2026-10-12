using HseKat.Domain.Abstractions;
using HseKat.Domain.Things;
using HseKat.Domain.Vehicles;
using HseKat.Services.Fleet;
using HseKat.Services.Inspection;
using NSubstitute;

namespace HseKat.Tests;

public class FleetServiceTests
{
    private readonly IServiceCenter _serviceCenter = Substitute.For<IServiceCenter>();
    private readonly IInventoryRepository _repository = Substitute.For<IInventoryRepository>();
    private readonly FleetService _fleetService;

    public FleetServiceTests()
    {
        _fleetService = new FleetService(_serviceCenter, _repository);
    }

    [Fact]
    public void AdmitVehicle_Rejected_ShouldNotAddToRepository()
    {
        var vehicle = new Bicycle(1, "Test", TechnicalState.Good, 8);
        _serviceCenter.Inspect(vehicle).Returns(InspectionResult.Rejected);

        _fleetService.AdmitVehicle(vehicle);

        _repository.DidNotReceive().Add(Arg.Any<IInventoryItem>());
    }

    [Fact]
    public void AdmitVehicle_Accepted_ShouldAddToRepositoryOnce()
    {
        var vehicle = new Bicycle(1, "Test", TechnicalState.Broken, 8);
        _serviceCenter.Inspect(vehicle).Returns(InspectionResult.Accepted);

        _fleetService.AdmitVehicle(vehicle);

        _repository.Received(1).Add(vehicle);
    }

    [Theory]
    [InlineData(InspectionResult.Accepted)]
    [InlineData(InspectionResult.Rejected)]
    public void AdmitVehicle_Call_ShouldReturnInspectionResult(InspectionResult expected)
    {
        var vehicle = new ElectricScooter(1, "Test", TechnicalState.Good, 7, 0.5);
        _serviceCenter.Inspect(vehicle).Returns(expected);

        var result = _fleetService.AdmitVehicle(vehicle);

        Assert.Equal(expected, result);
        _serviceCenter.Received(1).Inspect(vehicle);
    }

    [Fact]
    public void RegisterItem_Helmet_ShouldAddToRepositoryOnce()
    {
        var helmet = new Helmet(1, "Helmet");

        _fleetService.RegisterItem(helmet);

        _repository.Received(1).Add(helmet);
    }
}
