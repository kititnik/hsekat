using HseKat.Domain.Abstractions;

namespace HseKat.Domain.Things;

public sealed class Helmet : InventoryItem
{
    public Helmet(int inventoryNumber, string name)
        : base(inventoryNumber, name)
    {
    }
}
