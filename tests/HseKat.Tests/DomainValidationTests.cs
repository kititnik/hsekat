using HseKat.Domain.Things;
using HseKat.Domain.Vehicles;

namespace HseKat.Tests;

public class DomainValidationTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_NonPositiveInventoryNumber_ShouldThrow(int inventoryNumber)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Helmet(inventoryNumber, "Helmet"));
    }

    [Theory]
    [InlineData("")]
    public void Constructor_BlankName_ShouldThrow(string name)
    {
        Assert.Throws<ArgumentException>(() => new Helmet(1, name));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Constructor_BeginnerEaseOutOfRange_ShouldThrow(int ease)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Bicycle(1, "Test", TechnicalState.Good, ease));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void Constructor_InvalidDailyEnergy_ShouldThrow(double energy)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DockStation(1, "Dock", energy));
    }

    [Fact]
    public void Constructor_ValidArguments_ShouldSetProperties()
    {
        var scooter = new ElectricScooter(7, "Test", TechnicalState.NeedsRepair, 6, 0.5);

        Assert.Equal(7, scooter.InventoryNumber);
        Assert.Equal("Test", scooter.Name);
        Assert.Equal(TechnicalState.NeedsRepair, scooter.State);
        Assert.Equal(6, scooter.BeginnerEase);
        Assert.Equal(0.5, scooter.DailyEnergyKwh);
    }
}
