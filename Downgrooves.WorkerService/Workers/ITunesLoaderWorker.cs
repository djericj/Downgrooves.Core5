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
using Downgrooves.WorkerService.Extensions;

namespace Downgrooves.WorkerService.Workers
{
    public class ITunesLoaderWorker : BackgroundService
    {
        private readonly ILogger<ITunesLoaderWorker> _logger;
        private readonly AppConfig _appConfig;
        private readonly IApiClientService _clientService;
        private readonly IArtworkService _artworkService;

        public ITunesLoaderWorker(ILogger<ITunesLoaderWorker> logger,
            IOptions<AppConfig> config,
            IApiClientService clientService,
            IArtworkService artworkService)
        {
            _logger = logger;
            _appConfig = config.Value;
            _clientService = clientService;
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
                    await AddNewCollections(artist);
                    await AddNewCollectionsForRemixes(artist);
                }

                await AddNewTracks();
                await AddNewReleases();

                await _artworkService.GetArtwork("collections");
                await _artworkService.GetArtwork("tracks");

                _logger.LogInformation("Finished.");

                await Task.Delay(_appConfig.ITunes.PollInterval * 1000);
            }
        }

        private async Task AddNewCollections(string artistName)
        {
            var exclusions = await _clientService.GetExclusions();

            var collections = await _clientService.LookupCollections(artistName);

            var existingCollections = await _clientService.GetCollections();

            collections = collections.Where(x => exclusions.All(x2 => x2.CollectionId != x.CollectionId));

            if (existingCollections != null)
                collections = collections.Where(x => existingCollections.All(x2 => x2.CollectionId != x.CollectionId));

            collections = collections.GroupBy(x => x.CollectionId).Select(x => x.First()).ToList();

            if (collections != null && collections.Count() > 0)
            {
                _logger.LogInformation($"Adding {collections.Count()} collections.");
                await _clientService.AddNewCollections(collections.ToITunesCollections());
            }
        }

        private async Task AddNewCollectionsForRemixes(string artistName)
        {
            var tracks = await _clientService.LookupTracks(artistName);

            var collections = await _clientService.GetCollections();

            tracks = tracks
                .Where(x => x.WrapperType == "track")
                .GroupBy(x => x.CollectionId)
                .Select(x => x.First())
                .ToList();

            tracks = tracks.Where(x => collections.All(x2 => x2.CollectionId != x.CollectionId));

            if (tracks.Count() > 0)
            {
                _logger.LogInformation($"Adding {tracks.Count()} tracks.");
                await _clientService.AddNewCollections(tracks.ToITunesCollections());
            }
        }

        private async Task AddNewTracks()
        {
            var collections = await _clientService.GetCollections();
            var tracks = await _clientService.GetTracks();
            var items = new List<ITunesLookupResultItem>();
            foreach (var item in collections)
                items.AddRange(await _clientService.LookupTracksCollectionById(item.CollectionId));
            if (tracks != null)
                items = items.Where(x => tracks.All(x2 => x2.TrackId != x.TrackId)).ToList();

            if (items.Count() > 0)
            {
                _logger.LogInformation($"Adding {items.Count()} tracks for collections.");
                await _clientService.AddNewTracks(items.ToITunesTracks());
            }
        }

        private async Task AddNewReleases()
        {
            var releases = await _clientService.GetReleases();
            var collections = await _clientService.GetCollections();
            var tracks = await _clientService.GetTracks();
            var newReleases = new List<Release>();
            if (releases != null)
                collections = collections.Where(x => releases.All(x2 => x2.SourceSystemId != x.CollectionId)).ToList();
            foreach (var collection in collections)
            {
                var release = collection.ToRelease();
                release = await _clientService.AddNewRelease(release);
                release.Tracks = tracks.Where(x => x.CollectionId == collection.CollectionId).ToReleaseTracks(release.Id) as ICollection<ReleaseTrack>;
                await AddNewReleaseTracks(release.Tracks);
            }
            if (newReleases != null)
                await _clientService.AddNewReleases(newReleases);
        }

        private async Task AddNewReleaseTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            var count = await _clientService.AddNewReleaseTracks(releaseTracks);
            if (count > 0)
                _logger.LogInformation($"Adding {count} tracks for collection.");
        }
    }
}