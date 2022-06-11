using Downgrooves.Domain;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
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

        public async Task<JObject> GetResultsFromApi(string url, ApiData.ApiDataType type, string artist)
        {
            var data = await GetString(url.Replace("{searchTerm}", artist));
            var obj = JObject.Parse(data);
            await AddApiData(type, artist, url, obj);
            return obj;
        }

        public async Task<ApiData> AddApiData(ApiData data)
        {
            var content = "";
            var response = await ApiPost("data", data);
            if (response.IsSuccessful)
                content = response.Content;

            if (!string.IsNullOrEmpty(content))
                return JsonConvert.DeserializeObject<ApiData>(content);
            return null;
        }

        public async Task<ApiData> AddApiData(ApiData.ApiDataType type, string artist, string url, JObject obj)
        {
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
    }
}