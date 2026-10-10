using HseKat.Domain.Vehicles;
using HseKat.Services.Inspection;

namespace HseKat.Tests;

public class BasicServiceCenterTests
{
    [Theory]
    [InlineData(TechnicalState.Good, InspectionResult.Accepted)]
    [InlineData(TechnicalState.NeedsRepair, InspectionResult.Rejected)]
    [InlineData(TechnicalState.Broken, InspectionResult.Rejected)]
    public void Inspect_Call_ShouldReturnExpectedResult(TechnicalState state, InspectionResult expected)
    {
        var serviceCenter = new BasicServiceCenter();
        var vehicle = new Bicycle(1, "Test", state, 8);

        var result = serviceCenter.Inspect(vehicle);

        Assert.Equal(expected, result);
    }
}
