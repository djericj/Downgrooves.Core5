using Downgrooves.Domain;
using Newtonsoft.Json.Linq;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IReleaseService
    {
        Release GetRelease(string path, string type);
    }
}
