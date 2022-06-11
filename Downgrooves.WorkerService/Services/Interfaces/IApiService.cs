using Downgrooves.Domain;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IApiService
    {
        Task<JObject> Lookup(string url);

        Task<JObject> GetResultsFromApi(string url, ApiData.ApiDataType type, string artist);

        Task<ApiData> AddApiData(ApiData data);

        Task<ApiData> AddApiData(ApiData.ApiDataType type, string artist, string url, JObject obj);
    }
}