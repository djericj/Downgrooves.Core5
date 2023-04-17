using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
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
        private readonly IApiDataService _apiDataService;
        private readonly IITunesService _iTunesService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public ProcessWorker(IOptions<AppConfig> config,
            ILogger<ProcessWorker> logger,
            IApiDataService apiDataService,
            IITunesService iTunesService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _appConfig = config.Value;
            _logger = logger;
            _apiDataService = apiDataService;
            _iTunesService = iTunesService;
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

                        var lastChecked = GetLastCheckedFile();

                        if (lastChecked > DateTime.MinValue && DateTime.Now > lastChecked.AddDays(1))
                            GetDataFromITunesApi();
                        else
                        {
                            _logger.LogInformation($"{nameof(ProcessWorker)} last checked less than 24 hours ago ({lastChecked}).  Skipping.");
                        }

                        _iTunesService.GetData();

                        _iTunesService.GetArtwork();

                        WriteLastCheckedFile();

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

        private void GetDataFromITunesApi()
        {
            var artists = new[] { "Downgrooves", "Evotone", "Eric Rylos" };

            foreach (var artist in artists)
            {
                _logger.LogInformation($"{nameof(ProcessWorker)} getting {artist}.");
                _apiDataService.GetDataFromITunesApi(_appConfig.ITunes.CollectionSearchUrl, artist, ApiData.ApiDataTypes.iTunesCollection);
                _apiDataService.GetDataFromITunesApi(_appConfig.ITunes.TracksSearchUrl, artist, ApiData.ApiDataTypes.iTunesTrack);
            }
        }

        private DateTime GetLastCheckedFile()
        {
            var lastCheckedFile = new DirectoryInfo(_appConfig.JsonDataBasePath).GetFiles("last_checked*").FirstOrDefault();
            if (lastCheckedFile is { Exists: true })
            {
                if (DateTime.TryParse(File.ReadAllText(lastCheckedFile.FullName), out var lastCheckedDateTime))
                    return lastCheckedDateTime;
  
            }
            return DateTime.MinValue;

        }

        private void WriteLastCheckedFile()
        {
            var currentDateTime = DateTime.Now;
            var lastCheckedFilePath = Path.Combine(_appConfig.JsonDataBasePath, $"last_checked_{currentDateTime:yyyy-MM-dd}.txt");

            var lastCheckedFiles = new DirectoryInfo(_appConfig.JsonDataBasePath).GetFiles("last_checked*").Select(f => f.FullName);
            foreach (var lastCheckedFile in lastCheckedFiles)
                File.Delete(lastCheckedFile);

            File.WriteAllText(lastCheckedFilePath, currentDateTime.ToString(CultureInfo.InvariantCulture));
        }
    }
}