using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ITunesService : IITunesService
    {
        private readonly ILogger<IReleaseService> _logger;
        private IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;

        public ITunesService(ILogger<ReleaseService> logger, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<ITunesLookupResultItem> Add(ITunesLookupResultItem item)
        {
            try
            {
                await _unitOfWork.ITunes.AddAsync(item);
                await _unitOfWork.CompleteAsync();
                return item;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> AddRange(IEnumerable<ITunesLookupResultItem> items)
        {
            try
            {
                await _unitOfWork.ITunes.AddRangeAsync(items);
                await _unitOfWork.CompleteAsync();
                return items;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> GetItems()
        {
            return await _unitOfWork.ITunes.GetAllAsync();
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> Lookup(int Id)
        {
            try
            {
                var client = new RestClient(_configuration["AppConfig:ITunesLookupUrl"]);
                var request = new RestRequest();
                request.RequestFormat = DataFormat.Json;

                request.AddParameter("country", "us", ParameterType.UrlSegment);
                request.AddParameter("id", Id);
                request.AddParameter("entity", "musicArtist,musicTrack,album,mix,song");
                request.AddParameter("media", "music");
                var response = await client.ExecuteAsync<ITunesLookupResult>(request);
                if (!string.IsNullOrEmpty(response.Content))
                {
                    var lookupResult = JsonConvert.DeserializeObject<ITunesLookupResult>(response.Content);
                    return lookupResult.Results;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
            }
            return null;
        }
    }
}