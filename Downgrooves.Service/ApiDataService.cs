using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
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
                var exists = await GetApiData(data.DataType, data.Artist);
                if (exists != null && exists.Data == data.Data)
                {
                    _logger.LogInformation($"Data for {data.Artist} {data.DataType} is unchanged.");
                    exists.IsChanged = false;
                    exists.LastUpdate = DateTime.Now;
                    _unitOfWork.ApiData.UpdateState(exists);
                    await _unitOfWork.CompleteAsync();
                    return data;
                }
                else
                {
                    _logger.LogInformation($"Data for {data.Artist} {data.DataType} HAS changed.");
                    await _unitOfWork.ApiData.Remove(exists);
                    data.IsChanged = true;
                    data.LastUpdate = DateTime.Now;
                    await _unitOfWork.ApiData.AddAsync(data);
                    await _unitOfWork.CompleteAsync();
                    return data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ApiDataService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ApiData> Update(ApiData data)
        {
            try
            {
                _unitOfWork.ApiData.Update(data);
                await _unitOfWork.CompleteAsync();
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ApiDataService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ApiData> GetApiData(ApiData.ApiDataType dataType, string artist)
        {
            var apiData = await _unitOfWork.ApiData.FindAsync(x => (int)x.DataType == (int)dataType && x.Artist == artist);
            return apiData.FirstOrDefault();
        }

        public async Task ReloadData()
        {
        }
    }
}