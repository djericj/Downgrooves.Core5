using Downgrooves.Domain;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IApiService
    {
        Task<JObject> Lookup(string url);

        Task<List<ApiData>> GetResultsFromApi(string url, ApiData.ApiDataTypes type, string artist);

        Task<ApiData> AddApiData(ApiData data);

        Task<ApiData> UpdateApiData(ApiData data);
    }
}