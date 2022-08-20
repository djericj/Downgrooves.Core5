﻿using Downgrooves.Domain;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IApiDataService
    {
        JObject Lookup(string url);

        List<ApiData> UpdateDataFromITunesApi(string url, ApiData.ApiDataTypes type, string artist);

        ApiData AddApiData(ApiData data);

        IEnumerable<ApiData> GetApiData(ApiData.ApiDataTypes dataType, string artist);

        ApiData UpdateApiData(ApiData data);
    }
}