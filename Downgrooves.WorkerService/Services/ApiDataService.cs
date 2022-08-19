﻿using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Services
{
    public class ApiDataService : ApiService, IApiDataService
    {
        public ApiDataService(IOptions<AppConfig> config, ILogger<ApiService> logger) : base(config, logger)
        {
        }

        public JObject Lookup(string url)
        {
            var data = GetString(url);
            return JObject.Parse(data);
        }

        public List<ApiData> GetResultsFromApi(string url, ApiData.ApiDataTypes type, string artist)
        {
            List<ApiData> responses = new();
            int limit = 200;
            int offset = 0;

            url = url.Replace("{searchTerm}", artist);
            url = url.Replace("{limit}", limit.ToString());

            while (true)
            {
                if (offset > 0)
                    url += $"&offset={offset}";

                var data = GetString(url);
                var obj = JObject.Parse(data);
                var resultCount = Convert.ToInt32(obj["resultCount"]);

                if (resultCount == 0)
                    break;

                var apiData = new ApiData
                {
                    ApiDataType = type,
                    Artist = artist,
                    Url = url,
                    Total = resultCount,
                    Data = obj["results"].ToString(),
                    LastUpdate = DateTime.Now
                };

                apiData = AddApiData(apiData);
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

        public ApiData AddApiData(ApiData apiData)
        {
            var response = ApiPost<ApiData>(GetUri("data"), Token, apiData);
            if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                return JsonConvert.DeserializeObject<ApiData>(response.Content);
            return null;
        }

        public IEnumerable<ApiData> GetApiData(ApiData.ApiDataTypes dataType, string artist)
        {
            var response = ApiGet(GetUri($"data/{dataType}/artist/{artist}"), Token);
            if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                return JsonConvert.DeserializeObject<IEnumerable<ApiData>>(response.Content);
            return null;
        }

        public ApiData UpdateApiData(ApiData apiData)
        {
            var response = ApiPut<ApiData>(GetUri("data"), Token, apiData);
            if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                return JsonConvert.DeserializeObject<ApiData>(response.Content);
            return null;
        }
    }
}