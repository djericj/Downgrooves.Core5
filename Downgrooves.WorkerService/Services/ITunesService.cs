using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Downgrooves.WorkerService.Services
{
    public class ITunesService : ApiService, IITunesService
    {
        private readonly ILogger<ITunesService> _logger;

        public ITunesService(IOptions<AppConfig> config, ILogger<ITunesService> logger) : base(config, logger)
        {
            _logger = logger;
        }

        #region Downgrooves iTunes API

        #region Collections

        public ITunesCollection AddCollection(ITunesCollection collection)
        {
            return Add("itunes/collection", collection);
        }

        public IEnumerable<ITunesCollection> AddCollections(IEnumerable<ITunesCollection> collections)
        {
            return Add("itunes/collections", collections);
        }

        public ITunesCollection DeleteCollection(ITunesCollection collection)
        {
            return Delete("itunes/collection", collection);
        }

        public IEnumerable<ITunesCollection> DeleteCollections(IEnumerable<ITunesCollection> collections)
        {
            return Delete("itunes/collections", collections);
        }

        public ITunesCollection GetCollection(Artist artist = null)
        {
            return Get<ITunesCollection>("itunes/collection", artist);
        }

        public IEnumerable<ITunesCollection> GetCollections(Artist artist = null)
        {
            return Get<IEnumerable<ITunesCollection>>("itunes/collections", artist);
        }

        public ITunesCollection UpdateCollection(ITunesCollection collection)
        {
            return Update("itunes/collection", collection);
        }

        public IEnumerable<ITunesCollection> UpdateCollections(IEnumerable<ITunesCollection> collections)
        {
            return Update("itunes/collections", collections);
        }

        #endregion Collections

        #region Tracks

        public ITunesTrack AddTrack(ITunesTrack track)
        {
            return Add("itunes/track", track);
        }

        public IEnumerable<ITunesTrack> AddTracks(IEnumerable<ITunesTrack> tracks)
        {
            return Add("itunes/tracks", tracks);
        }

        public ITunesTrack GetTrack(Artist artist = null)
        {
            return Get<ITunesTrack>("itunes/track", artist);
        }

        public IEnumerable<ITunesTrack> GetTracks(Artist artist = null)
        {
            return Get<IEnumerable<ITunesTrack>>("itunes/tracks", artist);
        }

        public ITunesTrack UpdateTrack(ITunesTrack track)
        {
            return Update("itunes/track", track);
        }

        public IEnumerable<ITunesTrack> UpdateTracks(IEnumerable<ITunesTrack> tracks)
        {
            return Update("itunes/tracks", tracks);
        }

        public ITunesTrack DeleteTrack(ITunesTrack track)
        {
            return Delete("itunes/track", track);
        }

        public IEnumerable<ITunesTrack> DeleteTracks(IEnumerable<ITunesTrack> tracks)
        {
            return Delete("itunes/tracks", tracks);
        }

        #endregion Tracks

        private T Add<T>(string resource, T items)
        {
            var response = ApiPost<T>(GetUri(resource), Token, items);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error adding itunes items.  {response.Content}");
            return default;
        }

        private T Get<T>(string resource, Artist artist = null)
        {
            if (artist != null)
                resource += $"?artistName={artist.Name}";
            var response = ApiGet(GetUri(resource), Token);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes items.  {response.Content}");
            return default;
        }

        private T Update<T>(string resource, T items)
        {
            var response = ApiPut<T>(GetUri(resource), Token, items);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error updating itunes items.  {response.Content}");
            return default;
        }

        private T Delete<T>(string resource, T items)
        {
            var response = ApiDelete<T>(GetUri(resource), Token, items);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error updating itunes items.  {response.Content}");
            return default;
        }

        #endregion Downgrooves iTunes API
    }
}