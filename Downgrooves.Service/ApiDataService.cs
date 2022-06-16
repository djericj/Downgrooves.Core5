using Downgrooves.Model;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
                await ReloadData(data);
                return data;
            }
        }

        public async Task<ApiData> Update(ApiData data)
        {
            _unitOfWork.ApiData.Update(data);
            await _unitOfWork.CompleteAsync();
            return data;
        }

        public async Task<ApiData> GetApiData(ApiData.ApiDataType dataType, string artist)
        {
            var apiData = await _unitOfWork.ApiData.FindAsync(x => (int)x.DataType == (int)dataType && x.Artist == artist);
            return apiData.FirstOrDefault();
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

        private string GetEmbeddedResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}