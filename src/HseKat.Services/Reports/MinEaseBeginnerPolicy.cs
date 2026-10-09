using HseKat.Domain.Vehicles;

namespace HseKat.Services.Reports;

public sealed class MinEaseBeginnerPolicy : IBeginnerPolicy
{
    private readonly int _minEase;

    public MinEaseBeginnerPolicy(int minEase)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(minEase, Vehicle.MinBeginnerEase);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minEase, Vehicle.MaxBeginnerEase);

        _minEase = minEase;
    }

    public bool IsSuitable(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        return vehicle.BeginnerEase >= _minEase;
    }
}
