using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ApiDataService : IApiDataService
    {
        private readonly ILogger<IApiDataService> _logger;
        private IUnitOfWork _unitOfWork;

        public ApiDataService(ILogger<IApiDataService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiData> Add(ApiData data)
        {
            try
            {
                await _unitOfWork.ApiData.AddAsync(data);
                await _unitOfWork.CompleteAsync();
                return data;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ApiDataService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ApiData> GetApiData(ApiData.ApiDataType dataType)
        {
            var apiData = await _unitOfWork.ApiData.FindAsync(x => (int)x.DataType == (int)dataType);
            return apiData.FirstOrDefault();
        }

        public async Task<ApiData> Update(ApiData data)
        {
            try
            {
                _unitOfWork.ApiData.Update(data);
                await _unitOfWork.CompleteAsync();
                return data;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ApiDataService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task Remove(int id)
        {
            try
            {
                var data = await _unitOfWork.ApiData.GetAsync(id);
                await _unitOfWork.ApiData.Remove(data);
                await _unitOfWork.CompleteAsync();
                return;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ApiDataService.Remove {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}