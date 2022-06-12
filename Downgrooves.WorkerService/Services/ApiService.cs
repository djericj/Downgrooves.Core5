using Downgrooves.Domain;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ApiService : ApiBase, IApiService
    {
        public ApiService(IOptions<AppConfig> config) : base(config)
        {
        }

        public async Task<JObject> Lookup(string url)
        {
            var data = await GetString(url);
            return JObject.Parse(data);
        }

        public async Task<ApiData> GetResultsFromApi(string url, ApiData.ApiDataType type, string artist)
        {
            var data = await GetString(url.Replace("{searchTerm}", artist));
            var obj = JObject.Parse(data);
            var apiData = new ApiData();
            apiData.DataType = type;
            apiData.Artist = artist;
            apiData.Url = url;
            apiData.Total = Convert.ToInt32(obj["resultCount"]);
            apiData.Data = obj["results"].ToString();
            apiData.LastUpdate = DateTime.Now;
            await AddApiData(apiData);
            return apiData;
        }

        public async Task<ApiData> AddApiData(ApiData apiData)
        {
            var response = await ApiPost("data", apiData);
            if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                return JsonConvert.DeserializeObject<ApiData>(response.Content);
            return null;
        }

        public async Task<ApiData> UpdateApiData(ApiData apiData)
        {
            var response = await ApiPut<ApiData>("data", apiData);
            if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                return JsonConvert.DeserializeObject<ApiData>(response.Content);
            return null;
        }
    }
}