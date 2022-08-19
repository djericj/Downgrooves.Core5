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

namespace Downgrooves.WorkerService.Services
{
    public class ReleaseService : IReleaseService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<ReleaseService> _logger;
        private readonly IApiDataService _apiDataService;
        private readonly IArtistService _artistService;
        private readonly IArtworkService _artworkService;
        private readonly IITunesService _itunesService;

        public ReleaseService(IOptions<AppConfig> config,
            ILogger<ReleaseService> logger,
            IApiDataService apiDataService,
            IArtistService artistService,
            IArtworkService artworkService,
            IITunesService itunesService)
        {
            _appConfig = config.Value;
            _logger = logger;
            _apiDataService = apiDataService;
            _artistService = artistService;
            _artworkService = artworkService;
            _itunesService = itunesService;
        }

        public void ProcessData()
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

                    collections.AddRange(newItems.Where(c => c.WrapperType == "collection"));
                }
                else
                {
                    collections.AddRange(collection.Where(c => c.WrapperType == "collection"));
                }
            }

            _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {collections?.Count} TOTAL collections to evaluate.");

            if (collections?.Count > 0)
            {
                var existingCollections = _itunesService.GetCollections(new Artist() { Name = artist });

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {existingCollections?.Count()} EXISTing collections.");

                var newCollections = collections.Where(c => existingCollections.All(c2 => c2.Id != c.Id));

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {newCollections?.Count()} NEW collections.");

                var addedCollections = _itunesService.AddCollections(newCollections);

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {addedCollections?.Count()} ADDED collections.");
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

                    tracks.AddRange(newItems.Where(c => c.WrapperType == "track"));
                }
                else
                {
                    tracks.AddRange(track.Where(c => c.WrapperType == "track"));
                }
            }

            _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {tracks?.Count} TOTAL tracks to evaluate.");

            if (tracks?.Count > 0)
            {
                var existingTracks = _itunesService.GetTracks(new Artist() { Name = artist });

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {existingTracks?.Count()} EXISTing tracks.");

                var newTracks = tracks.Where(c => existingTracks.All(c2 => c2.Id != c.Id));

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {newTracks?.Count()} NEW tracks.");

                var addedTracks = _itunesService.AddTracks(newTracks);

                _logger.LogInformation($"{nameof(ReleaseService)} {artist}: {addedTracks?.Count()} ADDED tracks.");
            }

            _logger.LogInformation($"/************************* END ***************************/.");
            _logger.LogInformation("");
            _logger.LogInformation("");
        }

        private void DownloadCollectionsArtwork()
        {
            var imageFiles = GetImageFiles($@"{_appConfig.ArtworkBasePath}\collections");
            var collections = _itunesService.GetCollections();
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
            var imageFiles = GetImageFiles($@"{_appConfig.ArtworkBasePath}\tracks");
            var tracks = _itunesService.GetTracks();
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
    }
}