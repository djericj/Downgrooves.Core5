using Downgrooves.Domain;
using Newtonsoft.Json.Linq;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IApiDataService
    {
        JObject Lookup(string url);

        string LookupSongs(string id);

        void GetDataFromITunesApi(string url, string artist, ApiData.ApiDataTypes type);
    }
}