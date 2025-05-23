﻿using Downgrooves.WorkerService.Config;
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
        private readonly IITunesService _iTunesService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public ProcessWorker(IOptions<AppConfig> config,
            ILogger<ProcessWorker> logger,
            IITunesService iTunesService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _appConfig = config.Value;
            _logger = logger;
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

                        _iTunesService.CheckFolders();
                        
                        _iTunesService.GetDataFromITunesApi();

                        _iTunesService.GetData();

                        _iTunesService.GetArtwork();

                        _iTunesService.WriteLastCheckedFile();

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

        

        
    }
}