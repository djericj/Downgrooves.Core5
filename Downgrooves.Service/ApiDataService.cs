using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ApiDataService : ServiceBase, IApiDataService
    {
        private readonly ILogger<IApiDataService> _logger;

        public ApiDataService(IConfiguration configuration, ILogger<IApiDataService> logger, IUnitOfWork unitOfWork) :
            base(configuration, unitOfWork)
        {
            _logger = logger;
        }

        public async Task<ApiData> Add(ApiData data)
        {
            var exists = await GetApiData(data.ApiDataType, data.Artist);

            if (exists != null && exists.Any())
            {
                foreach (var item in exists)
                {
                    if (item.Data == data.Data && item.Total == data.Total && item.ApiDataType == data.ApiDataType)
                    {
                        _logger.LogInformation($"Data for {data.Artist} {data.ApiDataType} is unchanged.");
                    }
                    else if (item.Total != data.Total && item.ApiDataType == data.ApiDataType)
                    {
                        data = await AddNew(data);
                    }
                    else
                    {
                        data = await UpdateState(data);
                    }
                }
            }
            else
            {
                data = await AddNew(data);
            }

            return data;
        }

        private async Task<ApiData> AddNew(ApiData data)
        {
            _logger.LogInformation($"Data for {data.Artist} {data.ApiDataType} is NEW.");
            data.IsChanged = true;
            data.LastUpdate = DateTime.Now;
            await _unitOfWork.ApiData.AddAsync(data);
            await _unitOfWork.CompleteAsync();
            return data;
        }

        private async Task<ApiData> UpdateState(ApiData data)
        {
            _logger.LogInformation($"Data for {data.Artist} {data.ApiDataType} HAS changed.");
            data.IsChanged = true;
            data.LastUpdate = DateTime.Now;
            _unitOfWork.ApiData.UpdateState(data);
            await _unitOfWork.CompleteAsync();
            return data;
        }

        public async Task<ApiData> Update(ApiData data)
        {
            _unitOfWork.ApiData.Update(data);
            await _unitOfWork.CompleteAsync();
            return data;
        }

        public async Task<IEnumerable<ApiData>> GetApiData(ApiData.ApiDataTypes dataType, string artist)
        {
            return await _unitOfWork.ApiData.FindAsync(x => (int)x.ApiDataType == (int)dataType && x.Artist == artist);
        }

        public async Task ReloadData(ApiData data)
        {
            // reload itunes data
            await ExecuteSqlFromResource("GetITunesCollections.sql", data.Artist);
            await ExecuteSqlFromResource("GetITunesTracks.sql", data.Artist);
        }

        public async Task ExecuteSqlFromResource(string fileName, string artist)
        {
            var sql = GetEmbeddedResource(fileName);
            var result = await ExecuteSql(sql.Replace("@artistName", $"'{artist}'"));
            if (result == 0)
                _logger.LogWarning($"ExecuteSql {fileName} affected 0 rows.");
        }
    }
}