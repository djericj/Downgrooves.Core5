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
        private readonly IArtistService _artistService;
        private readonly IArtworkService _artworkService;
        private readonly IITunesLookupService _lookupService;
        private readonly IITunesService _iTunesService;
        private readonly IReleaseService _releaseService;

        public ITunesLoaderWorker(ILogger<ITunesLoaderWorker> logger,
            IOptions<AppConfig> config,
            IITunesLookupService lookupService,
            IITunesService iTunesService,
            IReleaseService releaseService,
            IArtistService artistService,
            IArtworkService artworkService)
        {
            _logger = logger;
            _appConfig = config.Value;
            _artistService = artistService;
            _artworkService = artworkService;
            _iTunesService = iTunesService;
            _lookupService = lookupService;
            _releaseService = releaseService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("ITunesLoaderWorker ticked at: {time}", DateTimeOffset.Now);

                var artists = await _artistService.GetArtists();

                foreach (var artist in artists)
                {
                    _logger.LogInformation($"Starting for {artist.Name}");
                    await AddNewCollections(artist);
                    await AddNewCollectionsForRemixes(artist);
                    await AddNewTracks(artist);
                    await AddNewReleases(artist);
                    _logger.LogInformation($"{artist.Name} finished.");
                }

                await _artworkService.GetArtwork("collections");
                await _artworkService.GetArtwork("tracks");

                _logger.LogInformation("Finished.");

                await Task.Delay(_appConfig.ITunes.PollInterval * 1000);
            }
        }

        private async Task AddNewCollections(Artist artist)
        {
            var exclusions = await _iTunesService.GetExclusions();

            var collections = await _lookupService.LookupCollections(artist.Name);

            var existingCollections = await _iTunesService.GetCollections(artist);

            collections = collections.Where(x => exclusions.All(x2 => x2.CollectionId != x.CollectionId));

            if (existingCollections != null)
                collections = collections.Where(x => existingCollections.All(x2 => x2.CollectionId != x.CollectionId));

            collections = collections.GroupBy(x => x.CollectionId).Select(x => x.First()).ToList();

            if (collections != null && collections.Count() > 0)
            {
                _logger.LogInformation($"Adding {collections.Count()} collections for {artist.Name}.");
                await _iTunesService.AddNewCollections(collections.ToITunesCollections(artist));
            }
        }

        private async Task AddNewCollectionsForRemixes(Artist artist)
        {
            var tracks = await _lookupService.LookupTracks(artist.Name);

            var collections = await _iTunesService.GetCollections();

            tracks = tracks
                .Where(x => x.WrapperType == "track")
                .GroupBy(x => x.CollectionId)
                .Select(x => x.First())
                .ToList();
            if (collections != null)
                tracks = tracks.Where(x => collections.All(x2 => x2.CollectionId != x.CollectionId));

            if (tracks.Count() > 0)
            {
                _logger.LogInformation($"Adding {tracks.Count()} tracks for {artist.Name}.");
                await _iTunesService.AddNewCollections(tracks.ToITunesCollections(artist));
            }
        }

        private async Task AddNewTracks(Artist artist)
        {
            var collections = await _iTunesService.GetCollections();
            var tracks = await _iTunesService.GetTracks(artist);
            var items = new List<ITunesLookupResultItem>();
            foreach (var item in collections)
                items.AddRange(await _lookupService.LookupTracksCollectionById(item.CollectionId));
            if (tracks != null)
                items = items.Where(x => tracks.All(x2 => x2.TrackId != x.TrackId)).ToList();

            items = items.GroupBy(x => x.TrackId).Select(x => x.First()).ToList();

            if (items.Count() > 0)
            {
                _logger.LogInformation($"Adding {items.Count()} tracks for collections for {artist.Name}.");
                await _iTunesService.AddNewTracks(items.ToITunesTracks(artist));
            }
        }

        private async Task AddNewReleases(Artist artist)
        {
            var releases = await _releaseService.GetReleases(artist);
            var collections = await _iTunesService.GetCollections(artist);
            var tracks = await _iTunesService.GetTracks(artist);
            if (releases != null)
                collections = collections.Where(x => releases.All(x2 => x2.SourceSystemId != x.CollectionId)).ToList();
            foreach (var collection in collections)
            {
                var release = collection.ToRelease(artist);

                release = await _releaseService.AddNewRelease(release);
                release.Tracks = tracks.Where(x => x.CollectionId == collection.CollectionId).ToReleaseTracks(release) as ICollection<ReleaseTrack>;
                await AddNewReleaseTracks(release.Tracks);
            }
        }

        private async Task AddNewReleaseTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            var count = await _releaseService.AddNewReleaseTracks(releaseTracks);
            if (count > 0)
                _logger.LogInformation($"Adding {count} tracks for collection.");
        }
    }
}