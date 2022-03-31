using Newtonsoft.Json.Linq;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IApiClientService
    {
        IJEnumerable<JToken> GetItunesJson(string searchTerm);
        IJEnumerable<JToken> GetYouTubeVideosJson();
    }
}
