using HseKat.Domain.Abstractions;

namespace HseKat.Domain.Vehicles;

public sealed class ElectricScooter : Vehicle, IEnergyConsumer
{
    public double DailyEnergyKwh { get; }

    public ElectricScooter(int inventoryNumber, string name, TechnicalState state, int beginnerEase, double dailyEnergyKwh)
        : base(inventoryNumber, name, state, beginnerEase)
    {
        if (!double.IsFinite(dailyEnergyKwh))
            throw new ArgumentOutOfRangeException(nameof(dailyEnergyKwh), dailyEnergyKwh, "Energy must be a finite number.");
        ArgumentOutOfRangeException.ThrowIfNegative(dailyEnergyKwh);

        DailyEnergyKwh = dailyEnergyKwh;
    }
}
