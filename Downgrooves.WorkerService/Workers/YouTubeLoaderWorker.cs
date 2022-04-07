using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Workers
{
    public class YouTubeLoaderWorker : BackgroundService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<YouTubeLoaderWorker> _logger;
        private readonly IYouTubeService _youTubeService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public YouTubeLoaderWorker(IOptions<AppConfig> config,
            ILogger<YouTubeLoaderWorker> logger,
            IYouTubeService youTubeService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _appConfig = config.Value;
            _logger = logger;
            _youTubeService = youTubeService;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(YouTubeLoaderWorker)} is starting.");
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("YouTubeLoaderWorker ticked at: {time}", DateTimeOffset.Now);

                    var newVideos = new List<Video>();
                    var videos = new List<Video>(await _youTubeService.GetYouTubeVideosJson());
                    var existingVideos = new List<Video>(await _youTubeService.GetExistingVideos());

                    if (existingVideos.Any())
                        newVideos = videos.Where(x => existingVideos.All(y => x.SourceSystemId != y.SourceSystemId)).ToList();
                    else
                        newVideos = videos;

                    if (newVideos.Any())
                    {
                        var count = await _youTubeService.AddNewVideos(newVideos);
                        if (count > 0)
                            _logger.LogInformation($"{count} videos added.");

                        await _artworkService.GetArtwork(newVideos);
                    }
                    else
                    {
                        _logger.LogInformation("No new vieos.");
                    }

                    await Task.Delay(_appConfig.YouTube.PollInterval * 1000);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }
    }
}