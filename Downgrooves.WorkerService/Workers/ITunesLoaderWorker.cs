using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Workers
{
    public class ITunesLoaderWorker : BackgroundService
    {
        private readonly ILogger<ITunesLoaderWorker> _logger;
        private readonly AppConfig _appConfig;
        private readonly IApiClientService _clientService;
        private readonly ICollectionService _collectionService;
        private readonly ITrackService _trackService;
        private readonly IArtworkService _artworkService;

        public ITunesLoaderWorker(ILogger<ITunesLoaderWorker> logger, 
            IOptions<AppConfig> config, 
            IApiClientService clientService,
            ICollectionService collectionService, 
            ITrackService trackService,
            IArtworkService artworkService)
        {
            _logger = logger;
            _appConfig = config.Value;
            _clientService = clientService;
            _collectionService = collectionService;
            _trackService = trackService;
            _artworkService = artworkService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("ITunesLoaderWorker ticked at: {time}", DateTimeOffset.Now);

                var artists = new string[] { "Downgrooves", "Eric Rylos", "Evotone" };

                foreach (var artist in artists)
                {
                    //get all collectons (albums) with the artist name (Downgrooves)
                    var collections = _clientService.LookupCollections(artist);
                    
                    _collectionService.AddCollections(collections);

                    //get all tracks with the artist name (Downgrooves)
                    var tracks = _clientService.LookupTracks(artist);

                    _trackService.AddTracks(tracks);

                    //go through each track

                    //Get the collection for each track

                }

                // get artwork for everything
                _artworkService.GetArtwork("collections");
                _artworkService.GetArtwork("tracks");

                await Task.Delay(_appConfig.ITunes.PollInterval * 1000);
            }
        }
    }
}
