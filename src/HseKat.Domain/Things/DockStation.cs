using HseKat.Domain.Abstractions;

namespace HseKat.Domain.Things;

public sealed class DockStation : Thing, IEnergyConsumer
{
    public double DailyEnergyKwh { get; }

    public DockStation(int inventoryNumber, string name, double dailyEnergyKwh)
        : base(inventoryNumber, name)
    {
        if (!double.IsFinite(dailyEnergyKwh))
            throw new ArgumentOutOfRangeException(nameof(dailyEnergyKwh), dailyEnergyKwh, "Energy must be a finite number.");
        ArgumentOutOfRangeException.ThrowIfNegative(dailyEnergyKwh);

        DailyEnergyKwh = dailyEnergyKwh;
    }
}
