using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
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
    public class ITunesLoaderWorker : BackgroundService
    {
        private readonly ILogger<ITunesLoaderWorker> _logger;
        private readonly AppConfig _appConfig;
        private readonly IApiClientService _clientService;
        private readonly IArtworkService _artworkService;
        private readonly IReleaseService _releaseService;

        public ITunesLoaderWorker(ILogger<ITunesLoaderWorker> logger,
            IOptions<AppConfig> config,
            IApiClientService clientService,
            IReleaseService releaseService,
            IArtworkService artworkService)
        {
            _logger = logger;
            _appConfig = config.Value;
            _clientService = clientService;
            _artworkService = artworkService;
            _releaseService = releaseService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("ITunesLoaderWorker ticked at: {time}", DateTimeOffset.Now);

                var artists = new string[] { "Downgrooves", "Eric Rylos", "Evotone" };

                foreach (var artist in artists)
                {
                    AddNewCollections(artist);

                    AddNewTracks(artist);
                }

                AddITunesItems();

                // get artwork for everything
                _artworkService.GetArtwork("collections");
                _artworkService.GetArtwork("tracks");

                await Task.Delay(_appConfig.ITunes.PollInterval * 1000);
            }
        }

        private void AddNewCollections(string artistName)
        {
            var exclusions = _clientService.GetExclusions();

            var collections = _clientService.LookupCollections(artistName);

            collections = collections.Where(x => exclusions.All(x2 => x2.CollectionId != x.CollectionId));

            if (collections != null)
                _releaseService.AddCollections(collections.ToReleases());
        }

        private void AddNewTracks(string artistName)
        {
            var tracks = _clientService.LookupTracks(artistName);

            _releaseService.AddTracks(tracks.ToReleases());
        }

        private void AddITunesItems()
        {
            var releases = _clientService.GetReleases();
            var existingItems = _clientService.GetITunesLookupResultItems();
            if (existingItems != null)
                releases = releases.Where(x => existingItems.All(x2 => x2.CollectionId != x.CollectionId));
            foreach (var release in releases)
            {
                var items = new List<ITunesLookupResultItem>();
                var tracks = _clientService.LookupTracksCollectionById(release.CollectionId);
                _clientService.AddNewITunesItems(tracks);
            }
        }
    }

    public static class ITunesLoaderWorkerExtensions
    {
        public static IList<Release> ToReleases(this IEnumerable<ITunesLookupResultItem> items)
        {
            var releases = new List<Release>();
            foreach (var item in items)
            {
                releases.Add(new Release()
                {
                    ArtistId = item.ArtistId,
                    ArtistName = item.ArtistName,
                    ArtistViewUrl = item.ArtistViewUrl,
                    ArtworkUrl100 = item.ArtworkUrl100,
                    ArtworkUrl30 = item.ArtworkUrl30,
                    ArtworkUrl60 = item.ArtworkUrl60,
                    CollectionCensoredName = item.CollectionCensoredName,
                    CollectionExplicitness = item.CollectionExplicitness,
                    CollectionId = item.CollectionId,
                    CollectionName = item.CollectionName,
                    CollectionPrice = item.CollectionPrice,
                    CollectionType = item.CollectionType,
                    CollectionViewUrl = item.CollectionViewUrl,
                    Copyright = item.Copyright,
                    Country = item.Country,
                    Currency = item.Currency,
                    DiscCount = item.DiscCount,
                    DiscNumber = item.DiscNumber,
                    IsStreamable = item.IsStreamable,
                    Kind = item.Kind,
                    PreviewUrl = item.PreviewUrl,
                    PrimaryGenreName = item.PrimaryGenreName,
                    ReleaseDate = item.ReleaseDate,
                    TrackCensoredName = item.TrackCensoredName,
                    TrackCount = item.TrackCount,
                    TrackExplicitness = item.TrackExplicitness,
                    TrackId = item.TrackId,
                    TrackName = item.TrackName,
                    TrackNumber = item.TrackNumber,
                    TrackPrice = item.TrackPrice,
                    TrackTimeMillis = item.TrackTimeMillis,
                    TrackViewUrl = item.TrackViewUrl,
                    WrapperType = item.WrapperType
                });
            }
            return releases;
        }
    }
}