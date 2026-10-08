using HseKat.Domain.Abstractions;

namespace HseKat.Domain.Vehicles;

public sealed class ElectricBicycle : Vehicle, IEnergyConsumer
{
    public double DailyEnergyKwh { get; }

    public ElectricBicycle(int inventoryNumber, string name, TechnicalState state, int beginnerEase, double dailyEnergyKwh)
        : base(inventoryNumber, name, state, beginnerEase)
    {
        if (!double.IsFinite(dailyEnergyKwh))
            throw new ArgumentOutOfRangeException(nameof(dailyEnergyKwh), dailyEnergyKwh, "Energy must be a finite number.");
        ArgumentOutOfRangeException.ThrowIfNegative(dailyEnergyKwh);

        DailyEnergyKwh = dailyEnergyKwh;
    }
}
