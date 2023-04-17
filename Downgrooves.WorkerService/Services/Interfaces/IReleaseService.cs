using Downgrooves.Domain;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IReleaseService
    {
        Release GetRelease(string path);
    }
}
