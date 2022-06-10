using Downgrooves.Domain;
using System.Threading.Tasks;
using static Downgrooves.Domain.ApiData;

namespace Downgrooves.Service.Interfaces
{
    public interface IApiDataService
    {
        Task<ApiData> GetApiData(ApiDataType dataType);

        Task<ApiData> Add(ApiData data);

        Task<ApiData> Update(ApiData data);

        Task Remove(int id);
    }
}