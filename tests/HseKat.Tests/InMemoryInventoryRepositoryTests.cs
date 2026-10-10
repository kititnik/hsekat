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
}
