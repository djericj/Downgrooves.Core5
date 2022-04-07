using Downgrooves.Domain;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ReleaseService : ApiBase, IReleaseService
    {
        private int index = 0;
        private readonly ILogger<ReleaseService> _logger;

        public ReleaseService(IOptions<AppConfig> config, ILogger<ReleaseService> logger) : base(config)
        {
            _logger = logger;
        }

        #region Downgrooves Release API

        public async Task<int> AddNewReleases(IEnumerable<Release> releases)
        {
            index = 0;
            foreach (var release in releases)
                await AddNewRelease(release);
            return index;
        }

        public async Task<Release> AddNewRelease(Release release)
        {
            try
            {
                var description = $"{release.ArtistName} - {release.Title}";
                var response = await ApiPost("release", release);
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
                return JsonConvert.DeserializeObject<Release>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<int> AddNewReleaseTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            index = 0;
            foreach (var track in releaseTracks)
                await AddNewReleaseTrack(track);
            return index;
        }

        public async Task<ReleaseTrack> AddNewReleaseTrack(ReleaseTrack releaseTrack)
        {
            try
            {
                var description = $"{releaseTrack.ArtistName} - {releaseTrack.Title}";
                var response = await ApiPost("release/track", releaseTrack);
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
                return JsonConvert.DeserializeObject<ReleaseTrack>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<Release>> GetReleases(Artist artist = null)
        {
            var resource = "releases";
            if (artist != null)
                resource += $"artistName={artist.Name}";
            var response = await ApiGet(resource);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<Release[]>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing releases.  {response.Content}");
            return null;
        }

        #endregion Downgrooves Release API
    }
}