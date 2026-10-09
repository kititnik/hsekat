using HseKat.Domain.Vehicles;

namespace HseKat.Services.Reports;

public interface IBeginnerPolicy
{
    bool IsSuitable(Vehicle vehicle);
}
