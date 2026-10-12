using HseKat.Domain.Abstractions;

namespace HseKat.Repository;

public sealed class InMemoryInventoryRepository : IInventoryRepository
{
    private readonly List<IInventoryItem> _items = [];
    private readonly HashSet<int> _inventoryNumbers = [];
    private int _lastInventoryNumber;

    public int NextInventoryNumber()
    {
        return ++_lastInventoryNumber;
    }

    public void Add(IInventoryItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (!_inventoryNumbers.Add(item.InventoryNumber))
            throw new InvalidOperationException($"Inventory number {item.InventoryNumber} is already taken.");

        _items.Add(item);
        _lastInventoryNumber = Math.Max(_lastInventoryNumber, item.InventoryNumber);
    }

    public IEnumerable<IInventoryItem> GetAll()
    {
        return _items.AsReadOnly();
    }
}
