using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ArtworkService : ApiBase, IArtworkService
    {
        private readonly ILogger<ArtworkService> _logger;
        public string ApiUrl { get; }
        public string Token { get; }
        public string ArtworkBasePath { get; }

        public ArtworkService(IOptions<AppConfig> config, ILogger<ArtworkService> logger) : base(config)
        {
            ApiUrl = config.Value.ApiUrl;
            Token = config.Value.Token;
            ArtworkBasePath = config.Value.ArtworkBasePath;
            _logger = logger;
        }

        public async Task GetArtwork(IEnumerable<ITunesTrack> tracks)
        {
            try
            {
                foreach (var item in tracks)
                    await GetArtwork(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        public async Task GetArtwork(IEnumerable<ITunesCollection> collections)
        {
            try
            {
                foreach (var item in collections)
                    await GetArtwork(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        private async Task GetArtwork(ITunesTrack track)
        {
            var fileName = track.TrackId.ToString();
            var imagePath = Path.Combine(ArtworkBasePath, "tracks", $"{fileName}.jpg");
            if (!File.Exists(imagePath))
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        await client.DownloadFileTaskAsync(new Uri(track.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
                        _logger.LogInformation($"Downloaded track artwork {imagePath}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                    }
                }
            }
        }

        private async Task GetArtwork(ITunesCollection collection)
        {
            var fileName = collection.CollectionId.ToString();
            var imagePath = Path.Combine(ArtworkBasePath, "collections", $"{fileName}.jpg");
            if (!File.Exists(imagePath))
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        await client.DownloadFileTaskAsync(new Uri(collection.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
                        _logger.LogInformation($"Downloaded collection artwork {imagePath}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                    }
                }
            }
        }
    }
}