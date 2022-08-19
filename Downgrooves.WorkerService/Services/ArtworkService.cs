using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Downgrooves.WorkerService.Services
{
    public class ArtworkService : ApiService, IArtworkService
    {
        private readonly ILogger<ArtworkService> _logger;

        public ArtworkService(IOptions<AppConfig> config, ILogger<ArtworkService> logger) : base(config, logger)
        {
            _logger = logger;
        }

        public void DownloadArtwork(IEnumerable<ITunesTrack> tracks)
        {
            try
            {
                foreach (var item in tracks)
                    DownloadArtwork(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        public void DownloadArtwork(IEnumerable<ITunesCollection> collections)
        {
            try
            {
                foreach (var item in collections)
                    DownloadArtwork(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        private void DownloadArtwork(ITunesTrack track)
        {
            var fileName = track.Id.ToString();
            var imagePath = Path.Combine(ArtworkBasePath, "tracks", $"{fileName}.jpg");
            if (!File.Exists(imagePath))
            {
                using WebClient client = new();
                try
                {
                    client.DownloadFileTaskAsync(new Uri(track.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
                    _logger.LogInformation($"Downloaded track artwork {imagePath}");
                    System.Threading.Thread.Sleep(5000); // wait 5 sec to prevent getting 429 Too Many Requests error from iTunes API
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                }
            }
        }

        private void DownloadArtwork(ITunesCollection collection)
        {
            var fileName = collection.Id.ToString();
            var imagePath = Path.Combine(ArtworkBasePath, "collections", $"{fileName}.jpg");
            if (!File.Exists(imagePath))
            {
                using WebClient client = new();
                try
                {
                    client.DownloadFileTaskAsync(new Uri(collection.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
                    _logger.LogInformation($"Downloaded collection artwork {imagePath}");
                    System.Threading.Thread.Sleep(5000); // wait 5 sec to prevent getting 429 Too Many Requests error from iTunes API
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