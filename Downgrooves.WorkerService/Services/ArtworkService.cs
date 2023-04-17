using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Downgrooves.Domain;

namespace Downgrooves.WorkerService.Services
{
    public class ArtworkService : ApiService, IArtworkService
    {
        private readonly ILogger<ArtworkService> _logger;

        public ArtworkService(IOptions<AppConfig> config, ILogger<ArtworkService> logger) : base(config, logger)
        {
            _logger = logger;
        }

        public void DownloadArtwork(IEnumerable<Release> releases)
        {
            try
            {
                foreach (var release in releases)
                    DownloadArtwork(release);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        private void DownloadArtwork(Release release)
        {
            var imagePath = Path.Combine(ArtworkBasePath, $"{release.Id}.jpg");
            if (!File.Exists(imagePath))
            {
                using HttpClient client = new();
                try
                {
                    var bytes = client.GetByteArrayAsync(release.ArtworkUrl100.Replace("100x100", "500x500")).Result;
                    File.WriteAllBytesAsync(imagePath, bytes);
                    _logger.LogInformation($"Downloaded artwork {imagePath}");
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