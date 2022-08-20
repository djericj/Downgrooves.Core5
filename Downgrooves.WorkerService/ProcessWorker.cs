using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService
{
    public class ProcessWorker : BackgroundService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<ProcessWorker> _logger;
        private readonly IApiDataService _apiDataService;
        private readonly IArtistService _artistService;
        private readonly IReleaseService _releaseService;
        private readonly IITunesService _iTunesService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public ProcessWorker(IOptions<AppConfig> config,
            ILogger<ProcessWorker> logger,
            IApiDataService apiDataService,
            IArtistService artistService,
            IReleaseService releaseService,
            IITunesService iTunesService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _appConfig = config.Value;
            _logger = logger;
            _apiDataService = apiDataService;
            _artistService = artistService;
            _iTunesService = iTunesService;
            _releaseService = releaseService;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                _logger.LogInformation($"{nameof(ProcessWorker)} is starting.");
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        _logger.LogInformation($"{nameof(ProcessWorker)} ticked at: {DateTimeOffset.Now}");

                        GetITunesJsonData();

                        ProcessITunesJsonData();

                        _logger.LogInformation($"{nameof(ProcessWorker)} finished.");

                        Thread.Sleep(_appConfig.PollInterval * 1000);
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
            }, stoppingToken);
        }

        private void GetITunesJsonData()
        {
            var artists = _artistService.GetArtists();

            foreach (var artist in artists)
            {
                _logger.LogInformation($"{nameof(ProcessWorker)} getting {artist.Name}.");
                _apiDataService.UpdateDataFromITunesApi(_appConfig.ITunes.CollectionLookupUrl, ApiData.ApiDataTypes.iTunesCollection, artist.Name);
                _apiDataService.UpdateDataFromITunesApi(_appConfig.ITunes.TracksLookupUrl, ApiData.ApiDataTypes.iTunesTrack, artist.Name);
            }
        }

        private void ProcessITunesJsonData()
        {
            _iTunesService.ProcessJsonData();
        }
    }
}