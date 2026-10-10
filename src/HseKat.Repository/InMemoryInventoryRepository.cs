using HseKat.Domain.Abstractions;

namespace HseKat.Repository;

public sealed class InMemoryInventoryRepository : IInventoryRepository
{
    private readonly List<IInventoryItem> _items = [];
    private readonly HashSet<int> _inventoryNumbers = [];

    public void Add(IInventoryItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (!_inventoryNumbers.Add(item.InventoryNumber))
            throw new InvalidOperationException($"Inventory number {item.InventoryNumber} is already taken.");

        _items.Add(item);
    }

    public IEnumerable<IInventoryItem> GetAll()
    {
        return _items.AsReadOnly();
    }
}
