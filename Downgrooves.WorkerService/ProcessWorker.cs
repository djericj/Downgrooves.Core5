using Downgrooves.Model;
using Downgrooves.Model.ITunes;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IArtworkService _artworkService;
        private readonly IITunesService _itunesService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public ProcessWorker(IOptions<AppConfig> config,
            ILogger<ProcessWorker> logger,
            IApiService apiService,
            IArtistService artistService,
            IArtworkService artworkService,
            IITunesService itunesService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _appConfig = config.Value;
            _logger = logger;
            _apiService = apiService;
            _artistService = artistService;
            _artworkService = artworkService;
            _itunesService = itunesService;
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
                        _logger.LogInformation($"{nameof(ProcessWorker)} getting {artist.Name}.");
                        await _apiService.GetResultsFromApi(_appConfig.ITunes.CollectionLookupUrl, ApiData.ApiDataType.iTunesCollection, artist.Name);
                        await _apiService.GetResultsFromApi(_appConfig.ITunes.TracksLookupUrl, ApiData.ApiDataType.iTunesTrack, artist.Name);
                    }

                    _logger.LogInformation($"{nameof(ProcessWorker)} getting any new artwork.");

                    await DownloadCollectionsArtwork();
                    await DownloadTracksArtwork();

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

        private async Task DownloadCollectionsArtwork()
        {
            var imageFiles = GetImageFiles($@"{_appConfig.ArtworkBasePath}\collections");
            var collections = await _itunesService.Get<ITunesCollection>("itunes/collections");
            var collectionFiles = collections.Select(x => $"{x.Id}.jpg");
            var newFiles = collectionFiles.Except(imageFiles).ToList();
            if (newFiles != null && newFiles.Count > 0)
            {
                var download = collections.Where(x => newFiles.Contains($"{x.Id}.jpg"));
                await _artworkService.DownloadArtwork(download);
                _logger.LogInformation($"Downloaded {newFiles.Count} new artwork files");
            }
        }

        private async Task DownloadTracksArtwork()
        {
            var imageFiles = GetImageFiles($@"{_appConfig.ArtworkBasePath}\tracks");
            var tracks = await _itunesService.Get<ITunesTrack>("itunes/tracks");
            var trackFiles = tracks.Select(x => $"{x.Id}.jpg");
            var newFiles = trackFiles.Except(imageFiles).ToList();
            if (newFiles != null && newFiles.Count > 0)
            {
                var download = tracks.Where(x => newFiles.Contains($"{x.Id}.jpg"));
                await _artworkService.DownloadArtwork(download);
                _logger.LogInformation($"Downloaded {newFiles.Count} new artwork files");
            }
        }

        private IEnumerable<string> GetImageFiles(string path)
        {
            var dir = new DirectoryInfo(path);
            return dir.GetFiles("*.jpg").ToList().Select(x => x.Name);
        }
    }
}