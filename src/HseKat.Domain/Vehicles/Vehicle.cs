using HseKat.Domain.Abstractions;

namespace HseKat.Domain.Vehicles;

public abstract class Vehicle : InventoryItem
{
    public const int MinBeginnerEase = 1;
    public const int MaxBeginnerEase = 10;

    public TechnicalState State { get; }
    public int BeginnerEase { get; }

    protected Vehicle(int inventoryNumber, string name, TechnicalState state, int beginnerEase)
        : base(inventoryNumber, name)
    {
        if (!Enum.IsDefined(state))
            throw new ArgumentOutOfRangeException(nameof(state), state, "Unknown technical state.");

        ArgumentOutOfRangeException.ThrowIfLessThan(beginnerEase, MinBeginnerEase);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(beginnerEase, MaxBeginnerEase);

        State = state;
        BeginnerEase = beginnerEase;
    }
}
