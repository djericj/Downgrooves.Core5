using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Workers
{
    public class ITunesLoaderWorker : BackgroundService
    {
        private readonly ILogger<ITunesLoaderWorker> _logger;
        private readonly AppConfig _appConfig;
        private readonly ICollectionService _collectionService;
        private readonly ITrackService _trackService;

        public ITunesLoaderWorker(ILogger<ITunesLoaderWorker> logger, IOptions<AppConfig> config, ICollectionService collectionService, ITrackService trackService)
        {
            _logger = logger;
            _appConfig = config.Value;
            _collectionService = collectionService;
            _trackService = trackService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("ITunesLoaderWorker running at: {time}", DateTimeOffset.Now);

                var artists = new string[] { "Downgrooves", "Eric Rylos", "Evotone" };

                foreach (var artist in artists)
                {
                    _collectionService.AddCollections(artist);
                    _trackService.AddTracks(artist);
                }

                await Task.Delay(_appConfig.ITunes.PollInterval);
            }
        }
    }
}
