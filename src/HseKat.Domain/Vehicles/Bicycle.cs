namespace HseKat.Domain.Vehicles;

public sealed class Bicycle : Vehicle
{
    public Bicycle(int inventoryNumber, string name, TechnicalState state, int beginnerEase)
        : base(inventoryNumber, name, state, beginnerEase)
    {
    }
}
