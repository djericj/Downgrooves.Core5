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
        private readonly IApiService _apiService;
        private readonly IArtistService _artistService;
        private readonly IITunesService _itunesService;
        private readonly IITunesLookupService _itunesLookupService;
        private readonly IYouTubeService _youTubeService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public ProcessWorker(IOptions<AppConfig> config,
            ILogger<ProcessWorker> logger,
            IApiService apiService,
            IArtistService artistService,
            IITunesLookupService itunesLookupService,
            IITunesService itunesService,
            IYouTubeService youTubeService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _appConfig = config.Value;
            _logger = logger;
            _apiService = apiService;
            _artistService = artistService;
            _itunesLookupService = itunesLookupService;
            _itunesService = itunesService;
            _youTubeService = youTubeService;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(ProcessWorker)} is starting.");
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation($"{nameof(ProcessWorker)} ticked at: {DateTimeOffset.Now}");

                    var artists = await _artistService.GetArtists();

                    foreach (var artist in artists)
                    {
                        await _apiService.GetResultsFromApi(_appConfig.ITunes.CollectionLookupUrl, Domain.ApiData.ApiDataType.iTunesCollection, artist.Name);
                        await _apiService.GetResultsFromApi(_appConfig.ITunes.TracksLookupUrl, Domain.ApiData.ApiDataType.iTunesTrack, artist.Name);
                    }

                    //_itunesService.Process();
                    //_youTubeService.Process();

                    _logger.LogInformation($"{nameof(ProcessWorker)} finished.");

                    await Task.Delay(_appConfig.PollInterval * 1000);
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