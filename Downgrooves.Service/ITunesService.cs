using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ITunesService : IITunesService
    {
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<ITunesService> _logger;
        private IConfiguration _configuration;

        public ITunesService(IUnitOfWork unitOfWork, ILogger<ITunesService> logger, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _configuration = configuration;
        }

        #region Collections

        public async Task<ITunesCollection> Add(ITunesCollection collection)
        {
            try
            {
                _unitOfWork.ITunesCollections.Add(collection);
                await _unitOfWork.CompleteAsync();
                return collection;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesCollection>> AddRange(IEnumerable<ITunesCollection> collections)
        {
            try
            {
                _unitOfWork.ITunesCollections.AddRange(collections);
                await _unitOfWork.CompleteAsync();
                return collections;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollection(Expression<Func<ITunesCollection, bool>> predicate)
        {
            var collections = new List<ITunesCollection>();
            var collection = await _unitOfWork.ITunesCollections.FindAsync(predicate);
            foreach (var item in collection)
            {
                var newCollection = item;
                newCollection.Tracks = await LookupTracks(item.CollectionId);
                collections.Add(newCollection);
            }
            return collections;
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(string artistName = null)
        {
            return await _unitOfWork.ITunesCollections.GetCollections(artistName);
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(PagingParameters parameters, string artistName = null)
        {
            return await _unitOfWork.ITunesCollections.GetCollections(parameters, artistName);
        }

        public async Task<IEnumerable<ITunesCollection>> LookupCollection(int collectionId)
        {
            var client = new RestClient(_configuration["AppConfig:ITunesLookupUrl"]);
            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;

            request.AddParameter("country", "us", ParameterType.UrlSegment);
            request.AddParameter("id", collectionId);
            request.AddParameter("entity", "musicArtist,musicTrack,album,mix,song");
            request.AddParameter("media", "music");
            var response = await client.ExecuteAsync<ITunesLookupCollectionResult>(request);
            if (!string.IsNullOrEmpty(response.Content))
            {
                var lookupResult = JsonConvert.DeserializeObject<ITunesLookupCollectionResult>(response.Content);
                return lookupResult.Results;
            }
            return null;

        }

        public async Task<ITunesCollection> Update(ITunesCollection collection)
        {
            try
            {
                _unitOfWork.ITunesCollections.UpdateState(collection);
                await _unitOfWork.CompleteAsync();
                return collection;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public void Remove(ITunesCollection collection)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Tracks

        public async Task<ITunesTrack> Add(ITunesTrack track)
        {
            try
            {
                _unitOfWork.ITunesTracks.Add(track);
                await _unitOfWork.CompleteAsync();
                return track;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesTrack>> AddRange(IEnumerable<ITunesTrack> tracks)
        {
            try
            {
                _unitOfWork.ITunesTracks.AddRange(tracks);
                await _unitOfWork.CompleteAsync();
                return tracks;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ITunesTrack>> GetTrack(Expression<Func<ITunesTrack, bool>> predicate)
        {
            return await _unitOfWork.ITunesTracks.FindAsync(predicate);
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(string artistName = null)
        {
            return await _unitOfWork.ITunesTracks.GetTracks(artistName);
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(PagingParameters parameters, string artistName = null)
        {
            return await _unitOfWork.ITunesTracks.GetTracks(parameters, artistName);
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracksByCollection(int collectionId)
        {
            return await _unitOfWork.ITunesTracks.FindAsync(x => x.CollectionId == collectionId);
        }

        public async Task<IEnumerable<ITunesTrack>> LookupTracks(int collectionId)
        {
            var client = new RestClient(_configuration["AppConfig:ITunesLookupUrl"]);
            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;

            request.AddParameter("country", "us", ParameterType.UrlSegment);
            request.AddParameter("id", collectionId);
            request.AddParameter("entity", "song");
            var response = await client.ExecuteAsync<ITunesLookupTrackResult>(request);
            if (!string.IsNullOrEmpty(response.Content))
            {
                var lookupResult = JsonConvert.DeserializeObject<ITunesLookupTrackResult>(response.Content);
                return lookupResult.Results;
            }
            return null;

        }

        public async Task<ITunesTrack> Update(ITunesTrack track)
        {
            try
            {
                _unitOfWork.ITunesTracks.UpdateState(track);
                await _unitOfWork.CompleteAsync();
                return track;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public void Remove(ITunesTrack track)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}