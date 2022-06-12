using Downgrooves.Domain;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IApiService
    {
        Task<JObject> Lookup(string url);

        Task<ApiData> GetResultsFromApi(string url, ApiData.ApiDataType type, string artist);

        Task<ApiData> AddApiData(ApiData data);

        Task<ApiData> UpdateApiData(ApiData data);
    }
}