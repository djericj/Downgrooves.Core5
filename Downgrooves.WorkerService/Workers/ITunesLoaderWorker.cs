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
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        private List<ITunesCollection> _collections;
        private IEnumerable<ITunesTrack> _tracks;

        private List<ITunesCollection> _existingCollections;
        private IEnumerable<Release> _existingReleases;

        public ITunesLoaderWorker(ILogger<ITunesLoaderWorker> logger,
            IOptions<AppConfig> config,
            IITunesLookupService lookupService,
            IITunesService iTunesService,
            IReleaseService releaseService,
            IArtistService artistService,
            IArtworkService artworkService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _appConfig = config.Value;
            _artistService = artistService;
            _artworkService = artworkService;
            _iTunesService = iTunesService;
            _lookupService = lookupService;
            _releaseService = releaseService;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(ITunesLoaderWorker)} is starting.");
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("ITunesLoaderWorker ticked at: {time}", DateTimeOffset.Now);

                    var artists = await _artistService.GetArtists();

                    _collections = new List<ITunesCollection>();
                    _tracks = new List<ITunesTrack>();

                    _existingCollections = new List<ITunesCollection>();
                    _existingReleases = await _releaseService.GetReleases();

                    foreach (var artist in artists)
                    {
                        _logger.LogInformation($"Starting for {artist.Name}");

                        var collections = await GetCollections(artist);
                        _collections.AddRange(collections.Where(x => _existingCollections.All(x2 => x2.CollectionId != x.CollectionId)));
                        _existingCollections = _collections;

                        var remixes = await GetCollectionsForRemixes(artist);
                        _collections.AddRange(remixes.Where(x => _existingCollections.All(x2 => x2.CollectionId != x.CollectionId)));
                        _existingCollections = _collections;

                        _logger.LogInformation($"{artist.Name} finished.");
                    }

                    _existingCollections = new List<ITunesCollection>(await _iTunesService.GetCollections());

                    _collections = _collections.Where(x => _existingCollections.All(x2 => x2.CollectionId != x.CollectionId)).ToList();

                    if (_collections.Any())
                    {
                        _tracks = await GetTracks(_collections);
                        _tracks = _tracks.GroupBy(x => x.TrackId).Select(x => x.First());

                        await _iTunesService.AddNewCollections(_collections);
                        await _iTunesService.AddNewTracks(_tracks);

                        await _artworkService.GetArtwork(_collections);
                        await _artworkService.GetArtwork(_tracks);

                        var releases = _collections.ToReleases();

                        releases = releases.Where(x => _existingReleases.All(x2 => x2.CollectionId != x.CollectionId)).ToList();

                        if (releases.Any())
                        {
                            releases = await GetTracks(releases);
                            await _releaseService.AddNewReleases(releases);
                        }
                        else
                        {
                            _logger.LogInformation("No new releases.");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No new collections.");
                    }

                    _logger.LogInformation("Finished.");

                    await Task.Delay(_appConfig.ITunes.PollInterval * 1000);
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

        private async Task<IEnumerable<ITunesCollection>> GetCollections(Artist artist)
        {
            var exclusions = await _iTunesService.GetExclusions();

            var collections = await _lookupService.LookupCollections(artist.Name);

            // remove duplicate collection ids
            collections = collections.Where(x => exclusions.All(x2 => x2.CollectionId != x.CollectionId));

            //remove pre-release items
            collections = collections.Where(x => x.ReleaseDate > Convert.ToDateTime("2000-01-01"));

            if (collections != null && collections.Count() > 0)
            {
                _logger.LogInformation($"Adding {collections.Count()} collections for {artist.Name}.");
                return collections.ToITunesCollections(artist);
            }
            return null;
        }

        private async Task<IEnumerable<ITunesCollection>> GetCollectionsForRemixes(Artist artist)
        {
            var tracks = await _lookupService.LookupTracks(artist.Name);

            tracks = tracks
                .Where(x => x.WrapperType == "track")
                .Where(x => x.ReleaseDate > Convert.ToDateTime("2000-01-01")) // ignore pre-release
                .GroupBy(x => x.CollectionId)
                .Select(x => x.First())
                .ToList();

            if (tracks.Count() > 0)
            {
                _logger.LogInformation($"Adding {tracks.Count()} tracks for {artist.Name}.");
                return tracks.ToITunesCollections(artist);
            }
            return null;
        }

        private async Task<IEnumerable<ITunesTrack>> GetTracks(IEnumerable<ITunesCollection> collections)
        {
            var items = new List<ITunesLookupResultItem>();
            foreach (var item in collections)
                items.AddRange(await _lookupService.LookupTracksCollectionById(item.CollectionId));

            if (items.Count() > 0)
            {
                _logger.LogInformation($"Adding {items.Count()} tracks for {collections.Count()} collections.");
                return items.ToITunesTracks();
            }
            return null;
        }

        private async Task<IList<Release>> GetTracks(IEnumerable<Release> releases)
        {
            await Task.Run(() =>
            {
                foreach (var release in releases)
                    release.Tracks = _tracks.Where(x => x.CollectionId == release.CollectionId).ToReleaseTracks(release) as ICollection<ReleaseTrack>;
            });
            return releases.ToList();
        }
    }
}