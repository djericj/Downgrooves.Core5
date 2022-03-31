using Downgrooves.Domain;
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
            return await _unitOfWork.ITunesCollections.FindAsync(predicate);
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(string artistName = null)
        {
            return await _unitOfWork.ITunesCollections.GetCollections(artistName);
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(PagingParameters parameters, string artistName = null)
        {
            return await _unitOfWork.ITunesCollections.GetCollections(parameters, artistName);
        }

        public async Task<ITunesCollection> LookupCollection(int collectionId)
        {
            var client = new RestClient(_configuration["AppConfig:ITunesLookupUrl"]);
            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;

            request.AddParameter("country", "us", ParameterType.UrlSegment);
            request.AddParameter("id", collectionId);
            request.AddParameter("entity", "musicArtist,musicTrack,album,mix,song");
            request.AddParameter("media", "music");
            var response = await client.ExecuteAsync<ITunesCollection>(request);
            var content = response.Content;
            if (!string.IsNullOrEmpty(content))
            {
                var lookupResult = JsonConvert.DeserializeObject<ITunesLookupResult>(content);
                return lookupResult.Results as ITunesCollection;
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

        public async Task<IEnumerable<ITunesTrack>> LookupTracks(int collectionId)
        {
            var client = new RestClient(_configuration["AppConfig:ITunesLookupUrl"]);
            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;

            request.AddParameter("country", "us", ParameterType.UrlSegment);
            request.AddParameter("id", collectionId);
            request.AddParameter("entity", "song");
            var response = await client.ExecuteAsync<ITunesTrack[]>(request);
            var content = response.Content;
            if (!string.IsNullOrEmpty(content))
            {
                var lookupResult = JsonConvert.DeserializeObject<ITunesLookupResult>(content);
                return lookupResult.Results as ITunesTrack[];
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