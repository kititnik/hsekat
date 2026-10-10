using HseKat.Domain.Vehicles;
using HseKat.Services.Reports;

namespace HseKat.Tests;

public class MinEaseBeginnerPolicyTests
{
    [Theory]
    [InlineData(5, false)]
    [InlineData(6, true)]
    [InlineData(10, true)]
    public void IsSuitable_Call_ShouldReturnExpectedResult(int ease, bool expected)
    {
        var policy = new MinEaseBeginnerPolicy(6);
        var vehicle = new Bicycle(1, "Test", TechnicalState.Good, ease);

        var result = policy.IsSuitable(vehicle);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Constructor_MinEaseOutOfRange_ShouldThrow(int minEase)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MinEaseBeginnerPolicy(minEase));
    }
}
