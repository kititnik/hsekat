using HseKat.Domain.Vehicles;

namespace HseKat.Service.Reports;

public interface IBeginnerPolicy
{
    bool IsSuitable(Vehicle vehicle);
}
