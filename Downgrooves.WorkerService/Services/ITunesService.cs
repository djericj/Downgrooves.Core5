using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Downgrooves.WorkerService.Services
{
    public class ITunesService : ApiService, IITunesService
    {
        private readonly ILogger<ITunesService> _logger;
        private readonly IApiDataService _apiDataService;
        private readonly IArtistService _artistService;
        private readonly IArtworkService _artworkService;

        private readonly string _artworkBasePath;

        public ITunesService(IOptions<AppConfig> config, ILogger<ITunesService> logger, IApiDataService apiDataService,
            IArtistService artistService, IArtworkService artworkService) : base(config, logger)
        {
            _logger = logger;
            _apiDataService = apiDataService;
            _artistService = artistService;
            _artworkService = artworkService;
            _artworkBasePath = config.Value.ArtworkBasePath;
        }

        #region iTunes JSON

        public void ProcessJsonData()
        {
            var artists = _artistService.GetArtists();

            foreach (var artist in artists)
            {
                ProcessCollections(artist.Name);
                ProcessTracks(artist.Name);
            }

            _logger.LogInformation($"{nameof(ProcessWorker)} getting any new artwork.");

            DownloadCollectionsArtwork();
            DownloadTracksArtwork();
        }

        private void ProcessCollections(string artist)
        {
            int index = 0;

            _logger.LogInformation($"{nameof(ReleaseService)} getting COLLECTIONS for {artist}.");

            _logger.LogInformation($"/************************* {artist.ToUpper()} ***************************/.");
            _logger.LogInformation("");
            _logger.LogInformation("");

            var dataList = _apiDataService.GetApiData(ApiData.ApiDataTypes.iTunesCollection, artist);

            _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {dataList?.Count()} total JSON files to process.");

            var collections = new List<ITunesCollection>();

            foreach (var data in dataList)
            {
                index++;

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: Processing data file #{index}.");

                var collection = JsonConvert.DeserializeObject<IEnumerable<ITunesCollection>>(data.Data);
                if (collections.Any())
                {
                    var newItems = collection.Where(c => collections.All(c2 => c2.Id != c.Id));

                    _logger.LogInformation($"{nameof(ReleaseService)} {artist}: Found {newItems?.Count()} new items in file #{index}.");

                    collections.AddRange(newItems.Where(c => string.Compare(c.WrapperType, "collection", true) == 0 && c.ArtistName.Contains(artist)));
                }
                else
                {
                    collections.AddRange(collection.Where(c => string.Compare(c.WrapperType, "collection", true) == 0 && c.ArtistName.Contains(artist)));
                }
            }

            _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {collections?.Count} TOTAL collections to evaluate.");

            if (collections?.Count > 0)
            {
                var existingCollections = GetCollections(new Artist() { Name = artist });

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {existingCollections?.Count()} EXISTing collections.");

                var newCollections = collections.Where(c => existingCollections.All(c2 => c2.Id != c.Id));

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {newCollections?.Count()} NEW collections.");

                if (newCollections?.Count() > 0)
                {
                    var addedCollections = AddCollections(newCollections);

                    _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {addedCollections?.Count()} ADDED collections.");
                }
            }

            _logger.LogInformation($"/************************* END ***************************/.");
            _logger.LogInformation("");
            _logger.LogInformation("");
        }

        private void ProcessTracks(string artist)
        {
            int index = 0;

            _logger.LogInformation($"{nameof(ReleaseService)} getting TRACKS for {artist}.");

            _logger.LogInformation($"/************************* {artist.ToUpper()} ***************************/.");
            _logger.LogInformation("");
            _logger.LogInformation("");

            var dataList = _apiDataService.GetApiData(ApiData.ApiDataTypes.iTunesTrack, artist);

            _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {dataList?.Count()} total JSON files to process.");

            var tracks = new List<ITunesTrack>();

            foreach (var data in dataList)
            {
                index++;

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: Processing data file #{index}.");

                var track = JsonConvert.DeserializeObject<IEnumerable<ITunesTrack>>(data.Data);
                if (tracks.Any())
                {
                    var newItems = track.Where(c => tracks.All(c2 => c2.Id != c.Id));

                    _logger.LogInformation($"{nameof(ReleaseService)} {artist}: Found {newItems?.Count()} new items in file #{index}.");

                    tracks.AddRange(newItems.Where(c => string.Compare(c.WrapperType, "track", true) == 0));
                }
                else
                {
                    tracks.AddRange(track.Where(c => string.Compare(c.WrapperType, "track", true) == 0));
                }
            }

            _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {tracks?.Count} TOTAL tracks to evaluate.");

            if (tracks?.Count > 0)
            {
                var existingTracks = GetTracks();

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {existingTracks?.Count()} EXISTing tracks.");

                var newTracks = tracks.Where(c => existingTracks.All(c2 => c2.Id != c.Id && c2.CollectionId != c.CollectionId));

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {newTracks?.Count()} NEW tracks.");

                if (newTracks?.Count() > 0)
                {
                    var addedTracks = AddTracks(newTracks);

                    _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {addedTracks?.Count()} ADDED tracks.");
                }
            }

            _logger.LogInformation($"/************************* END ***************************/.");
            _logger.LogInformation("");
            _logger.LogInformation("");
        }

        private void DownloadCollectionsArtwork()
        {
            var imageFiles = GetImageFiles($@"{_artworkBasePath}\collections");
            var collections = GetCollections();
            var collectionFiles = collections.Select(x => $"{x.Id}.jpg");
            var newFiles = collectionFiles.Except(imageFiles).ToList();
            if (newFiles != null && newFiles.Count > 0)
            {
                var download = collections.Where(x => newFiles.Contains($"{x.Id}.jpg"));
                _artworkService.DownloadArtwork(download);
                _logger.LogInformation($"Downloaded {newFiles.Count} new artwork files");
            }
        }

        private void DownloadTracksArtwork()
        {
            var imageFiles = GetImageFiles($@"{_artworkBasePath}\tracks");
            var tracks = GetTracks();
            var trackFiles = tracks.Select(x => $"{x.Id}.jpg");
            var newFiles = trackFiles.Except(imageFiles).ToList();
            if (newFiles != null && newFiles.Count > 0)
            {
                var download = tracks.Where(x => newFiles.Contains($"{x.Id}.jpg"));
                _artworkService.DownloadArtwork(download);
                _logger.LogInformation($"Downloaded {newFiles.Count} new artwork files");
            }
        }

        private static IEnumerable<string> GetImageFiles(string path)
        {
            var dir = new DirectoryInfo(path);
            return dir.GetFiles("*.jpg").ToList().Select(x => x.Name);
        }

        #endregion iTunes JSON

        #region Downgrooves iTunes API

        #region Collections

        public ITunesCollection AddCollection(ITunesCollection collection)
        {
            return Add("itunes/collection", collection);
        }

        public IEnumerable<ITunesCollection> AddCollections(IEnumerable<ITunesCollection> collections)
        {
            return Add("itunes/collections", collections);
        }

        public ITunesCollection DeleteCollection(ITunesCollection collection)
        {
            return Delete("itunes/collection", collection);
        }

        public IEnumerable<ITunesCollection> DeleteCollections(IEnumerable<ITunesCollection> collections)
        {
            return Delete("itunes/collections", collections);
        }

        public ITunesCollection GetCollection(Artist artist = null)
        {
            return Get<ITunesCollection>("itunes/collection", artist);
        }

        public IEnumerable<ITunesCollection> GetCollections(Artist artist = null)
        {
            return Get<IEnumerable<ITunesCollection>>("itunes/collections", artist);
        }

        public ITunesCollection UpdateCollection(ITunesCollection collection)
        {
            return Update("itunes/collection", collection);
        }

        public IEnumerable<ITunesCollection> UpdateCollections(IEnumerable<ITunesCollection> collections)
        {
            return Update("itunes/collections", collections);
        }

        #endregion Collections

        #region Tracks

        public ITunesTrack AddTrack(ITunesTrack track)
        {
            return Add("itunes/track", track);
        }

        public IEnumerable<ITunesTrack> AddTracks(IEnumerable<ITunesTrack> tracks)
        {
            return Add("itunes/tracks", tracks);
        }

        public ITunesTrack GetTrack(Artist artist = null)
        {
            return Get<ITunesTrack>("itunes/track", artist);
        }

        public IEnumerable<ITunesTrack> GetTracks(Artist artist = null)
        {
            return Get<IEnumerable<ITunesTrack>>("itunes/tracks", artist);
        }

        public ITunesTrack UpdateTrack(ITunesTrack track)
        {
            return Update("itunes/track", track);
        }

        public IEnumerable<ITunesTrack> UpdateTracks(IEnumerable<ITunesTrack> tracks)
        {
            return Update("itunes/tracks", tracks);
        }

        public ITunesTrack DeleteTrack(ITunesTrack track)
        {
            return Delete("itunes/track", track);
        }

        public IEnumerable<ITunesTrack> DeleteTracks(IEnumerable<ITunesTrack> tracks)
        {
            return Delete("itunes/tracks", tracks);
        }

        #endregion Tracks

        #region Generics

        private T Add<T>(string resource, T items)
        {
            var response = ApiPost<T>(GetUri(resource), Token, items);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error adding itunes items.  {response.Content}");
            return default;
        }

        private T Get<T>(string resource, Artist artist = null)
        {
            if (artist != null)
                resource += $"?artistName={artist.Name}";
            var response = ApiGet(GetUri(resource), Token);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes items.  {response.Content}");
            return default;
        }

        private T Update<T>(string resource, T items)
        {
            var response = ApiPut<T>(GetUri(resource), Token, items);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error updating itunes items.  {response.Content}");
            return default;
        }

        private T Delete<T>(string resource, T items)
        {
            var response = ApiDelete<T>(GetUri(resource), Token, items);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error updating itunes items.  {response.Content}");
            return default;
        }

        #endregion Generics

        #endregion Downgrooves iTunes API
    }
}