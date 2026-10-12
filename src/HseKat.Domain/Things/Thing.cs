using HseKat.Domain.Abstractions;

namespace HseKat.Domain.Things;

public abstract class Thing : InventoryItem
{
    protected Thing(int inventoryNumber, string name)
        : base(inventoryNumber, name)
    {
    }
}
