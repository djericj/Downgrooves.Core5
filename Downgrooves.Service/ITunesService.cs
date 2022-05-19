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

        public async Task<ITunesCollection> Add(ITunesCollection item)
        {
            try
            {
                await _unitOfWork.ITunesCollection.AddAsync(item);
                await _unitOfWork.CompleteAsync();
                return item;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesCollection>> AddRange(IEnumerable<ITunesCollection> items)
        {
            try
            {
                await _unitOfWork.ITunesCollection.AddRangeAsync(items);
                await _unitOfWork.CompleteAsync();
                return items;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ITunesCollection> Update(ITunesCollection item)
        {
            try
            {
                _unitOfWork.ITunesCollection.UpdateState(item);
                await _unitOfWork.CompleteAsync();
                return item;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(string artistName = null)
        {
            if (artistName != null)
                return await _unitOfWork.ITunesCollection.FindAsync(x => x.ArtistName.Contains(artistName));
            else
                return await _unitOfWork.ITunesCollection.GetAllAsync();
        }

        public async Task<ITunesTrack> Add(ITunesTrack item)
        {
            try
            {
                await _unitOfWork.ITunesTrack.AddAsync(item);
                await _unitOfWork.CompleteAsync();
                return item;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesTrack>> AddRange(IEnumerable<ITunesTrack> items)
        {
            try
            {
                await _unitOfWork.ITunesTrack.AddRangeAsync(items);
                await _unitOfWork.CompleteAsync();
                return items;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(string artistName = null)
        {
            if (artistName != null)
                return await _unitOfWork.ITunesTrack.FindAsync(x => x.TrackCensoredName.Contains(artistName));
            else
                return await _unitOfWork.ITunesTrack.GetAllAsync();
        }

        public async Task<IEnumerable<ITunesExclusion>> GetExclusions()
        {
            return await _unitOfWork.ITunesExclusion.GetAllAsync();
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