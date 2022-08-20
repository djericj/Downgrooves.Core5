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

        public ApiData Add(ApiData apiData)
        {
            var existing = GetApiData(apiData.ApiDataType, apiData.Artist);

            if (existing != null && existing.Any())
            {
                var exists = false;

                foreach (var item in existing)
                {
                    exists = item.Total == apiData.Total &&
                        item.Url == apiData.Url &&
                        item.ApiDataType == apiData.ApiDataType;

                    if (exists)
                    {
                        if (item.Data.Trim() != apiData.Data.Trim())
                        {
                            item.LastUpdated = DateTime.Now;
                            Update(item);
                        }
                        break;
                    }
                }
                if (!exists)
                {
                    apiData.LastUpdated = DateTime.Now;
                    apiData = AddNew(apiData);
                }
            }
            else
            {
                apiData.LastUpdated = DateTime.Now;
                apiData = AddNew(apiData);
            }

            return apiData;
        }

        private ApiData AddNew(ApiData apiData)
        {
            _logger.LogInformation($"Data for {apiData.Artist} {apiData.ApiDataType} is NEW.");
            apiData.IsChanged = true;
            apiData.LastUpdated = DateTime.Now;
            _unitOfWork.ApiData.Add(apiData);
            _unitOfWork.Complete();
            return apiData;
        }

        public ApiData Update(ApiData apiData)
        {
            _unitOfWork.ApiData.Update(apiData);
            _unitOfWork.Complete();
            return apiData;
        }

        public IEnumerable<ApiData> GetApiData(ApiData.ApiDataTypes dataType, string artist)
        {
            return _unitOfWork.ApiData.Find(x => (int)x.ApiDataType == (int)dataType && x.Artist == artist);
        }

        public void ReloadData(ApiData apiData)
        {
            // reload itunes data
            ExecuteSqlFromResource("GetITunesCollections.sql", apiData.Artist);
            ExecuteSqlFromResource("GetITunesTracks.sql", apiData.Artist);
        }

        public void ExecuteSqlFromResource(string fileName, string artist)
        {
            var sql = GetEmbeddedResource(fileName);
            var result = ExecuteSql(sql.Replace("@artistName", $"'{artist}'"));
            if (result == 0)
                _logger.LogWarning($"ExecuteSql {fileName} affected 0 rows.");
        }
    }
}