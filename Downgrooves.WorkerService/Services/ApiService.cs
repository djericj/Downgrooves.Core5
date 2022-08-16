using Downgrooves.Domain;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        public async Task<List<ApiData>> GetResultsFromApi(string url, ApiData.ApiDataTypes type, string artist)
        {
            List<ApiData> responses = new List<ApiData>();
            int limit = 200;
            int offset = 0;

            url = url.Replace("{searchTerm}", artist);
            url = url.Replace("{limit}", limit.ToString());

            while (true)
            {
                if (offset > 0)
                    url += $"&offset={offset}";

                var data = await GetString(url);
                var obj = JObject.Parse(data);
                var resultCount = Convert.ToInt32(obj["resultCount"]);

                if (resultCount == 0)
                    break;

                var apiData = new ApiData();
                apiData.ApiDataType = type;
                apiData.Artist = artist;
                apiData.Url = url;
                apiData.Total = resultCount;
                apiData.Data = obj["results"].ToString();
                apiData.LastUpdate = DateTime.Now;

                apiData = await AddApiData(apiData);
                responses.Add(apiData);

                if (resultCount > offset)
                {
                    offset += 100;
                    System.Threading.Thread.Sleep(5000);
                }
                else
                    break;
            }

            return responses;
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