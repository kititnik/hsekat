namespace HseKat.Domain.Abstractions;

public interface IInventoryRepository
{
    void Add(IInventoryItem item);
    IEnumerable<IInventoryItem> GetAll();
}
