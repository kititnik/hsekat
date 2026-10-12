using HseKat.Domain.Abstractions;
using HseKat.Domain.Things;
using HseKat.Domain.Vehicles;
using HseKat.Repository;

namespace HseKat.Tests;

public class InMemoryInventoryRepositoryTests
{
    private readonly InMemoryInventoryRepository _repository = new();

    [Fact]
    public void Add_DuplicateInventoryNumber_ShouldThrow()
    {
        var helmet = new Helmet(1, "Helmet");
        _repository.Add(helmet);

        Assert.Throws<InvalidOperationException>(
            () => _repository.Add(new Bicycle(1, "Test", TechnicalState.Good, 8)));

        Assert.Same(helmet, Assert.Single(_repository.GetAll()));
    }

    [Fact]
    public void NextInventoryNumber_Call_ShouldReturnSequentialNumbers()
    {
        Assert.Equal(1, _repository.NextInventoryNumber());
        Assert.Equal(2, _repository.NextInventoryNumber());
    }

    [Fact]
    public void NextInventoryNumber_AfterAddWithHigherNumber_ShouldContinueFromIt()
    {
        _repository.Add(new Helmet(5, "Helmet"));

        var result = _repository.NextInventoryNumber();

        Assert.Equal(6, result);
    }
}
