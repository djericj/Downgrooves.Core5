using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ITunesService : ApiBase, IITunesService
    {
        private int index = 0;
        private readonly ILogger<ITunesService> _logger;

        public ITunesService(IOptions<AppConfig> config, ILogger<ITunesService> logger) : base(config)
        {
            _logger = logger;
        }

        #region Downgrooves iTunes API

        public async Task<IEnumerable<ITunesExclusion>> GetExclusions()
        {
            var response = await ApiGet("itunes/exclusions");
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IEnumerable<ITunesExclusion>>(response.Content);
            else
                _logger.LogError($"Error getting exclusions.  {response.Content}");
            return null;
        }

        public async Task<IEnumerable<ITunesCollection>> AddNewCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                var response = await ApiPost("itunes/collections", items);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {items.Count()} collections.");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {items.Count()} collections.  {response.Content}");
                return JsonConvert.DeserializeObject<IEnumerable<ITunesCollection>>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<ITunesCollection> AddNewCollection(ITunesCollection item)
        {
            try
            {
                var description = $"{item.ArtistName} - {item.CollectionName} ({item.CollectionId})";
                var response = await ApiPost("itunes/collection", item);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {description}");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {description}.  {response.Content}");
                return JsonConvert.DeserializeObject<ITunesCollection>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(Artist artist = null)
        {
            var resource = "itunes/collections";
            if (artist != null)
                resource += $"?artistName={artist.Name}";
            var response = await ApiGet(resource);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<ITunesCollection[]>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes collections.  {response.Content}");
            return null;
        }

        public async Task<IEnumerable<ITunesTrack>> AddNewTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                var response = await ApiPost("itunes/tracks", items);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {items.Count()} tracks.");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {items.Count()} items.  {response.Content}");
                return JsonConvert.DeserializeObject<IEnumerable<ITunesTrack>>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<ITunesTrack> AddNewTrack(ITunesTrack item)
        {
            try
            {
                var description = $"{item.ArtistName} - {item.CollectionName} ({item.CollectionId})";
                var response = await ApiPost("itunes/track", item);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {description}");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {description}.  {response.Content}");
                return JsonConvert.DeserializeObject<ITunesTrack>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(Artist artist)
        {
            var response = await ApiGet($"itunes/tracks?artistName={artist.Name}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<ITunesTrack[]>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes tracks.  {response.Content}");
            return null;
        }

        #endregion Downgrooves iTunes API
    }
}