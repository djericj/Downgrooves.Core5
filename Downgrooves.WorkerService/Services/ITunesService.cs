using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Extensions;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ITunesService : ApiBase, IITunesService
    {
        private int index = 0;
        private readonly ILogger<ITunesService> _logger;

        private readonly IITunesLookupService _lookupService;
        private readonly IArtistService _artistService;
        private readonly IArtworkService _artworkService;
        private readonly IReleaseService _releaseService;

        private List<ITunesCollection> _collections;
        private IEnumerable<ITunesTrack> _tracks;

        private List<ITunesCollection> _existingCollections;
        private IEnumerable<Release> _existingReleases;

        public ITunesService(IOptions<AppConfig> config,
            ILogger<ITunesService> logger,
            IITunesLookupService lookupService,
            IArtistService artistService,
            IReleaseService releaseService,
            IArtworkService artworkService
            ) : base(config)
        {
            _logger = logger;
            _lookupService = lookupService;
            _artistService = artistService;
            _releaseService = releaseService;
            _artworkService = artworkService;
        }

        public async void Process()
        {
            var artists = await _artistService.GetArtists();

            _collections = new List<ITunesCollection>();
            _tracks = new List<ITunesTrack>();

            _existingCollections = new List<ITunesCollection>();
            _existingReleases = await _releaseService.GetReleases();

            foreach (var artist in artists)
            {
                _logger.LogInformation($"Starting for {artist.Name}");

                var collections = await GetCollectionsFromApi(artist);
                _collections.AddRange(collections.Where(x => _existingCollections.All(x2 => x2.CollectionId != x.CollectionId)));
                //_existingCollections = _collections;

                var remixes = await GetCollectionsForRemixes(artist);
                _collections.AddRange(remixes.Where(x => _existingCollections.All(x2 => x2.CollectionId != x.CollectionId)));
                //_existingCollections = _collections;

                _logger.LogInformation($"{artist.Name} finished.");
            }

            _existingCollections = new List<ITunesCollection>(await GetCollectionsFromApi());

            _collections = _collections.Where(x => _existingCollections.All(x2 => x2.CollectionId != x.CollectionId)).ToList();

            if (_collections.Any())
            {
                _tracks = await GetTracks(_collections);
                _tracks = _tracks.GroupBy(x => x.TrackId).Select(x => x.First());

                await AddNewCollections(_collections);
                await AddNewTracks(_tracks);

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
        }

        #region Downgrooves iTunes API

        public async Task<IEnumerable<ITunesExclusion>> GetExclusions()
        {
            var response = await ApiGet("itunes/exclusions");
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IEnumerable<ITunesExclusion>>(response.Content);
            else
                _logger.LogError($"Error getting exclusions.  {response.Content}");
            return null;
        }

        public async Task<IEnumerable<ITunesCollection>> AddNewCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                var response = await ApiPost("itunes/collections", items);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {items.Count()} collections.");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {items.Count()} collections.  {response.Content}");
                return JsonConvert.DeserializeObject<IEnumerable<ITunesCollection>>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<ITunesCollection> AddNewCollection(ITunesCollection item)
        {
            try
            {
                var description = $"{item.ArtistName} - {item.CollectionName} ({item.CollectionId})";
                var response = await ApiPost("itunes/collection", item);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {description}");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {description}.  {response.Content}");
                return JsonConvert.DeserializeObject<ITunesCollection>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollectionsFromApi(Artist artist = null)
        {
            var resource = "itunes/collections";
            if (artist != null)
                resource += $"?artistName={artist.Name}";
            var response = await ApiGet(resource);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<ITunesCollection[]>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes collections.  {response.Content}");
            return null;
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(Artist artist)
        {
            var exclusions = await GetExclusions();

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

        public async Task<IEnumerable<ITunesCollection>> GetCollectionsForRemixes(Artist artist)
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

        public async Task<IEnumerable<ITunesTrack>> GetTracks(IEnumerable<ITunesCollection> collections)
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

        public async Task<IList<Release>> GetTracks(IEnumerable<Release> releases)
        {
            await Task.Run(() =>
            {
                foreach (var release in releases)
                    release.Tracks = _tracks.Where(x => x.CollectionId == release.CollectionId).ToReleaseTracks(release) as ICollection<ReleaseTrack>;
            });
            return releases.ToList();
        }

        public async Task<IEnumerable<ITunesTrack>> AddNewTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                var response = await ApiPost("itunes/tracks", items);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {items.Count()} tracks.");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {items.Count()} items.  {response.Content}");
                return JsonConvert.DeserializeObject<IEnumerable<ITunesTrack>>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<ITunesTrack> AddNewTrack(ITunesTrack item)
        {
            try
            {
                var description = $"{item.ArtistName} - {item.CollectionName} ({item.CollectionId})";
                var response = await ApiPost("itunes/track", item);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {description}");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {description}.  {response.Content}");
                return JsonConvert.DeserializeObject<ITunesTrack>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(Artist artist = null)
        {
            var resource = "itunes/tracks";
            if (artist != null)
                resource += $"?artistName={artist.Name}";
            var response = await ApiGet(resource);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<ITunesTrack[]>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes tracks.  {response.Content}");
            return null;
        }

        #endregion Downgrooves iTunes API
    }
}