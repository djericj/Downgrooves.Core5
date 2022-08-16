using Downgrooves.Domain;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IApiDataService
    {
        Task<ApiData> GetApiData(ApiData.ApiDataTypes dataType, string artist);

        Task<ApiData> Add(ApiData data);

        Task<ApiData> Update(ApiData data);

        Task ReloadData(ApiData data);
    }
}