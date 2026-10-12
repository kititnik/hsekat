namespace HseKat.Domain.Abstractions;

public interface IInventoryRepository
{
    int NextInventoryNumber();
    void Add(IInventoryItem item);
    IEnumerable<IInventoryItem> GetAll();
}
