using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Downgrooves.WorkerService.Services
{
    public class ITunesService : ApiService, IITunesService
    {
        private readonly ILogger<ITunesService> _logger;
        private readonly IApiDataService _apiDataService;
        private readonly IArtistService _artistService;
        private readonly IArtworkService _artworkService;

        private readonly string _artworkBasePath;
        private readonly List<ITunesCollection> _collections;
        private readonly List<ITunesTrack> _tracks;

        public ITunesService(IOptions<AppConfig> config, ILogger<ITunesService> logger, IApiDataService apiDataService,
            IArtistService artistService, IArtworkService artworkService) : base(config, logger)
        {
            _logger = logger;
            _apiDataService = apiDataService;
            _artistService = artistService;
            _artworkService = artworkService;
            _artworkBasePath = config.Value.ArtworkBasePath;

            _collections = new List<ITunesCollection>();
            _tracks = new List<ITunesTrack>();
        }

        #region iTunes JSON

        public void ProcessJsonData()
        {
            Process(_apiDataService.GetApiData());

            var lookupLog = GetLookupLog();
            var searchCollections = GetCollections();
            var searchTracks = GetTracks();

            if (lookupLog != null)
            {
                searchCollections = searchCollections.Where(x => lookupLog.All(l => l.CollectionId != x.Id)).GroupBy(x => x.Id).Select(g => g.First());
                searchTracks = searchTracks.Where(x => lookupLog.All(l => l.CollectionId != x.CollectionId)).GroupBy(x => x.CollectionId).Select(g => g.First());
            }

            foreach (var item in searchCollections)
            {
                _logger.LogInformation($"{nameof(ITunesService)} looking up collection {item.Id}.");
                var data = _apiDataService.LookupSongs(item.Id);
                JObject jsonData = JObject.Parse(data);
                var apiData = new ApiData()
                {
                    ApiDataType = ApiData.ApiDataTypes.iTunesLookup,
                    CollectionId = item.Id,
                    Data = jsonData.SelectToken("$.results").ToString(),
                    LastUpdated = System.DateTime.Now,
                    Url = $"{LookupUrl}?id={item.Id}&entity=song",
                };
                Process(new List<ApiData>() { apiData });
                _apiDataService.AddApiData(apiData);
                AddLookupLog(item.Id);
                _logger.LogInformation($"{nameof(ITunesService)} waiting {LookupInterval} seconds.");
                System.Threading.Thread.Sleep(LookupInterval * 1000);
            }

            foreach (var item in searchTracks)
            {
                _logger.LogInformation($"{nameof(ITunesService)} looking up track in collection {item.CollectionId}.");
                var data = _apiDataService.LookupSongs(item.CollectionId);
                JObject jsonData = JObject.Parse(data);
                var apiData = new ApiData()
                {
                    ApiDataType = ApiData.ApiDataTypes.iTunesLookup,
                    CollectionId = item.CollectionId,
                    Data = jsonData.SelectToken("$.results").ToString(),
                    LastUpdated = System.DateTime.Now,
                    Url = $"{LookupUrl}?id={item.Id}&entity=song",
                };
                Process(new List<ApiData>() { apiData });
                _apiDataService.AddApiData(apiData);
                AddLookupLog(item.CollectionId);
                _logger.LogInformation($"{nameof(ITunesService)} waiting {LookupInterval} seconds.");
                System.Threading.Thread.Sleep(LookupInterval * 1000);
            }

            _logger.LogInformation($"{nameof(ProcessWorker)} getting any new artwork.");

            DownloadCollectionsArtwork();
            DownloadTracksArtwork();
        }

        private void Process(IEnumerable<ApiData> dataList)
        {
            if (dataList == null) return;

            var collections = new List<ITunesCollection>();
            var tracks = new List<ITunesTrack>();

            foreach (var data in dataList)
            {
                JArray objData = JArray.Parse(data.Data);

                var collectionsJson = objData.SelectTokens("$.[?(@.wrapperType == 'collection')]");
                foreach (var item in collectionsJson)
                {
                    collections.Add(item.ToObject<ITunesCollection>());
                }

                var tracksJson = objData.SelectTokens("$.[?(@.wrapperType == 'track')]");
                foreach (var item in tracksJson)
                {
                    tracks.Add(item.ToObject<ITunesTrack>());
                }
            }

            if (collections?.Count > 0)
            {
                var existingCollections = GetCollections();
                var newCollections = _collections.Where(c => existingCollections.All(c2 => c2.Id != c.Id));

                if (newCollections?.Count() > 0)
                {
                    _logger.LogInformation($"{nameof(ITunesService)} processing collections {string.Join(",", newCollections.Select(s => s.Id))}.");
                    AddCollections(newCollections);
                }
            }

            if (tracks?.Count > 0)
            {
                var existingTracks = GetTracks();
                var newTracks = _tracks.Where(c => existingTracks.All(c2 => c2.Id != c.Id)).GroupBy(g => g.Id).Select(s => s.First());

                if (newTracks?.Count() > 0)
                {
                    _logger.LogInformation($"{nameof(ITunesService)} processing tracks {string.Join(",", newTracks.Select(s => s.Id))}.");
                    AddTracks(newTracks);
                }
            }
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

        public IEnumerable<ITunesLookupLog> GetLookupLog()
        {
            return Get<IEnumerable<ITunesLookupLog>>("itunes/lookup");
        }

        public ITunesLookupLog GetLookupLog(int id)
        {
            return Get<ITunesLookupLog>($"itunes/lookup/{id}");
        }

        public ITunesLookupLog AddLookupLog(int id)
        {
            var item = new ITunesLookupLog() { CollectionId = id, LastUpdated = System.DateTime.Now };
            return Add("itunes/lookup", item);
        }

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

        #endregion Downgrooves iTunes API
    }
}