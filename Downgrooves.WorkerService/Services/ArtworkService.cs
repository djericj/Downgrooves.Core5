using Downgrooves.Model.ITunes;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
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

        public async Task DownloadArtwork(IEnumerable<ITunesTrack> tracks)
        {
            try
            {
                foreach (var item in tracks)
                    await DownloadArtwork(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        public async Task DownloadArtwork(IEnumerable<ITunesCollection> collections)
        {
            try
            {
                foreach (var item in collections)
                    await DownloadArtwork(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        private async Task DownloadArtwork(ITunesTrack track)
        {
            var fileName = track.Id.ToString();
            var imagePath = Path.Combine(ArtworkBasePath, "tracks", $"{fileName}.jpg");
            if (!File.Exists(imagePath))
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        await client.DownloadFileTaskAsync(new Uri(track.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
                        _logger.LogInformation($"Downloaded track artwork {imagePath}");
                        await Task.Delay(1000); // wait 1 sec to prevent getting 429 Too Many Requests error from iTunes API
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                    }
                }
            }
        }

        private async Task DownloadArtwork(ITunesCollection collection)
        {
            var fileName = collection.Id.ToString();
            var imagePath = Path.Combine(ArtworkBasePath, "collections", $"{fileName}.jpg");
            if (!File.Exists(imagePath))
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        await client.DownloadFileTaskAsync(new Uri(collection.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
                        _logger.LogInformation($"Downloaded collection artwork {imagePath}");
                        await Task.Delay(1000); // wait 1 sec to prevent getting 429 Too Many Requests error from iTunes API
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