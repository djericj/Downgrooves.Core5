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

        public async Task<ITunesCollection> AddCollection(ITunesCollection item)
        {
            try
            {
                await _unitOfWork.ITunesCollection.AddAsync(item);
                await _unitOfWork.CompleteAsync();
                return item;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddCollection {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesCollection>> AddCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                await _unitOfWork.ITunesCollection.AddRangeAsync(items);
                await _unitOfWork.CompleteAsync();
                return items;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddCollections {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ITunesTrack> AddTrack(ITunesTrack item)
        {
            try
            {
                await _unitOfWork.ITunesTrack.AddAsync(item);
                await _unitOfWork.CompleteAsync();
                return item;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddTrack {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesTrack>> AddTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                await _unitOfWork.ITunesTrack.AddRangeAsync(items);
                await _unitOfWork.CompleteAsync();
                return items;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddTracks {ex.Message} {ex.StackTrace}");
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

        public async Task<ITunesCollection> GetCollection(int id)
        {
            return await _unitOfWork.ITunesCollection.GetAsync(id);
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(string artistName = null)
        {
            if (artistName != null)
                return await _unitOfWork.ITunesTrack.FindAsync(x => x.TrackCensoredName.Contains(artistName));
            else
                return await _unitOfWork.ITunesTrack.GetAllAsync();
        }

        public async Task<ITunesTrack> GetTrack(int id)
        {
            return await _unitOfWork.ITunesTrack.GetAsync(id);
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

        public async Task RemoveCollection(int id)
        {
            try
            {
                var collection = await _unitOfWork.ITunesCollection.GetAsync(id);
                await _unitOfWork.ITunesCollection.Remove(collection);
                await _unitOfWork.CompleteAsync();
                return;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.RemoveCollection {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveTrack(int id)
        {
            try
            {
                var track = await _unitOfWork.ITunesTrack.GetAsync(id);
                await _unitOfWork.ITunesTrack.Remove(track);
                await _unitOfWork.CompleteAsync();
                return;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.RemoveTrack {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveCollections(IEnumerable<int> ids)
        {
            try
            {
                foreach (var item in ids)
                    await RemoveCollection(item);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.RemoveCollections {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveTracks(IEnumerable<int> ids)
        {
            try
            {
                foreach (var item in ids)
                    await RemoveTrack(item);
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.RemoveTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ITunesCollection> UpdateCollection(ITunesCollection item)
        {
            try
            {
                _unitOfWork.ITunesCollection.UpdateState(item);
                await _unitOfWork.CompleteAsync();
                return item;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.UpdateCollection {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ITunesTrack> UpdateTrack(ITunesTrack item)
        {
            try
            {
                _unitOfWork.ITunesTrack.UpdateState(item);
                await _unitOfWork.CompleteAsync();
                return item;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.UpdateTrack {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesCollection>> UpdateCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                foreach (var item in items)
                    await UpdateCollection(item);
                return items;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.UpdateCollections {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesTrack>> UpdateTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                foreach (var item in items)
                    await UpdateTrack(item);
                return items;
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.UpdateTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}