using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ApiDataService : IApiDataService
    {
        private readonly ILogger<IApiDataService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ApiDataService(ILogger<IApiDataService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
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

        private async Task<int> ExecuteSql(string sql)
        {
            return await _unitOfWork.ExecuteNonQueryAsync(sql);
        }

        private static string GetEmbeddedResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
    }
}