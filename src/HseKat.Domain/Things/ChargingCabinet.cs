using HseKat.Domain.Abstractions;

namespace HseKat.Domain.Things;

public sealed class ChargingCabinet : InventoryItem, IEnergyConsumer
{
    public double DailyEnergyKwh { get; }

    public ChargingCabinet(int inventoryNumber, string name, double dailyEnergyKwh)
        : base(inventoryNumber, name)
    {
        if (!double.IsFinite(dailyEnergyKwh))
            throw new ArgumentOutOfRangeException(nameof(dailyEnergyKwh), dailyEnergyKwh, "Energy must be a finite number.");
        ArgumentOutOfRangeException.ThrowIfNegative(dailyEnergyKwh);

        DailyEnergyKwh = dailyEnergyKwh;
    }
}
