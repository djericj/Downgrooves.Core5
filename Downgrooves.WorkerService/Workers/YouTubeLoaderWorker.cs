using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Workers
{
    public class YouTubeLoaderWorker : BackgroundService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<YouTubeLoaderWorker> _logger;
        private readonly IYouTubeService _youTubeService;

        public YouTubeLoaderWorker(IOptions<AppConfig> config, ILogger<YouTubeLoaderWorker> logger, IYouTubeService youTubeService)
        {
            _appConfig = config.Value;
            _logger = logger;
            _youTubeService = youTubeService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("YouTubeLoaderWorker ticked at: {time}", DateTimeOffset.Now);
                await _youTubeService.AddNewVideos();
                await Task.Delay(_appConfig.YouTube.PollInterval * 1000);
            }
        }
    }
}