using Downgrooves.Domain;

namespace Downgrooves.Service.Interfaces
{
    public interface IHealthService
    {
        HealthReport GetHealth();
    }
}
