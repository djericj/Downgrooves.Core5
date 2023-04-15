using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Service.Interfaces
{
    public interface IApiDataService
    {
        IEnumerable<ApiData> GetApiData();

        IEnumerable<ApiData> GetApiData(ApiData.ApiDataTypes dataType);

        IEnumerable<ApiData> GetApiData(ApiData.ApiDataTypes dataType, string artist);

        ApiData Add(ApiData apiData);

        ApiData Update(ApiData apiData);

        void ReloadData(ApiData apiData);
    }
}