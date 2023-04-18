using Downgrooves.Domain;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AppConfig = Downgrooves.WorkerService.Config.AppConfig;

namespace Downgrooves.WorkerService.Services
{
    public class ITunesService : ApiService, IITunesService
    {
        private readonly IOptions<AppConfig> _config;
        private readonly ILogger<ITunesService> _logger;
        private readonly IApiDataService _apiDataService;
        private readonly IArtworkService _artworkService;
        private readonly IReleaseService _releaseService;

        private readonly string _artworkBasePath;

        public ITunesService(IOptions<AppConfig> config, ILogger<ITunesService> logger, IApiDataService apiDataService, IArtworkService artworkService, IReleaseService releaseService) : base(config, logger)
        {
            _config = config;   
            _logger = logger;
            _apiDataService = apiDataService;
            _artworkService = artworkService;
            _releaseService = releaseService;   
            _artworkBasePath = config.Value.ArtworkBasePath;
        }

        public void CheckFolders()
        {
            var basePath = _config.Value.JsonDataBasePath;
            CheckFolder(basePath);
            CheckFolder(Path.Combine(basePath, "itunes"));
            CheckFolder(Path.Combine(basePath, "itunes", "collections", "artists"));
            CheckFolder(Path.Combine(basePath, "itunes", "tracks", "artists"));
        }

        public void GetData()
        {
            var basePath = _config.Value.JsonDataBasePath;
            var existingFiles = GetExistingFiles(Path.Combine(basePath, "itunes"), ".json").ToList();
            var searchArtistCollections = GetIds(Path.Combine(basePath, "itunes", "collections", "artists"));
            var searchArtistTracks = GetIds(Path.Combine(basePath, "itunes", "tracks", "artists"));
            var newArtistCollections = searchArtistCollections.Except(existingFiles.Select(f => f.Replace(".json",""))).ToList();
            var newArtistTracks = searchArtistTracks.Except(existingFiles.Select(f => f.Replace(".json", ""))).ToList();

            if (newArtistCollections.Count == 0 && newArtistTracks.Count == 0)
            {
                _logger.LogInformation($"{nameof(ProcessWorker)} no new items.");
            }
            else
            {
                DownloadData(newArtistCollections, Path.Combine($"{_config.Value.JsonDataBasePath}", "itunes"));
                DownloadData(newArtistTracks, Path.Combine($"{_config.Value.JsonDataBasePath}", "itunes"));
            }
        }

        public void GetArtwork()
        {
            _logger.LogInformation($"{nameof(ProcessWorker)} looking for new artwork.");

            var existingFiles = GetExistingFiles(Path.Combine(_config.Value.JsonDataBasePath, "itunes"), ".json").ToList();
            var existingArtwork = GetExistingFiles(_artworkBasePath, ".jpg").ToList();
            var newArtworkItems = existingFiles.Select(e => e.Replace(".json", "")).Except(existingArtwork.Select(a => a.Replace(".jpg", ""))).ToList();

            if (newArtworkItems.Count == 0)
                _logger.LogInformation($"{nameof(ProcessWorker)} no new artwork.");
            else
                DownloadArtwork(newArtworkItems.Select(a => a.Replace(".json","")), _artworkBasePath);
        }

        public void GetDataFromITunesApi()
        {
            var artists = new[] { "Downgrooves", "Evotone", "Eric Rylos" };
            var basePath = _config.Value.JsonDataBasePath;
            var paths = new[]
            {
                Path.Combine(basePath, "itunes", "collections", "artists"),
                Path.Combine(basePath, "itunes", "tracks", "artists")
            };

            var exists = artists.All(artist => paths.All(path => File.Exists(Path.Combine(path, $"{artist}.json"))));

            var lastChecked = GetLastCheckedFile();

            if (lastChecked == DateTime.MinValue || DateTime.Now > lastChecked.AddDays(1) || !exists)
            {
                foreach (var artist in artists)
                {
                    _logger.LogInformation($"{nameof(ProcessWorker)} getting {artist}.");
                    _apiDataService.GetDataFromITunesApi(_config.Value.ITunes.CollectionSearchUrl, artist, ApiData.ApiDataTypes.iTunesCollection);
                    _apiDataService.GetDataFromITunesApi(_config.Value.ITunes.TracksSearchUrl, artist, ApiData.ApiDataTypes.iTunesTrack);
                }
            }
            else
            {
                _logger.LogInformation($"{nameof(ProcessWorker)} last checked less than 24 hours ago ({lastChecked}).  Skipping.");
            }
        }

        public DateTime GetLastCheckedFile()
        {
            var lastCheckedFile = new DirectoryInfo(_config.Value.JsonDataBasePath).GetFiles("last_checked*").FirstOrDefault();
            if (lastCheckedFile is { Exists: true })
            {
                if (DateTime.TryParse(File.ReadAllText(lastCheckedFile.FullName), out var lastCheckedDateTime))
                    return lastCheckedDateTime;

            }
            return DateTime.MinValue;

        }

        public void WriteLastCheckedFile()
        {
            var currentDateTime = DateTime.Now;
            var lastCheckedFilePath = Path.Combine(_config.Value.JsonDataBasePath, $"last_checked_{currentDateTime:yyyy-MM-dd}.txt");

            var lastCheckedFiles = new DirectoryInfo(_config.Value.JsonDataBasePath).GetFiles("last_checked*").Select(f => f.FullName);
            foreach (var lastCheckedFile in lastCheckedFiles)
                File.Delete(lastCheckedFile);

            File.WriteAllText(lastCheckedFilePath, currentDateTime.ToString(CultureInfo.InvariantCulture));
        }

        private static IEnumerable<string> GetIds(string path)
        {
            var directory = new DirectoryInfo(path);
            var idList = new List<string>();

            foreach (var file in directory.GetFiles("*.json"))
            {
                var data = File.ReadAllText(file.FullName);
                var obj = JArray.Parse(data);
                var ids = obj.SelectTokens("$..collectionId").Select(j => j.Value<string>());
                idList.AddRange(ids);
            }

            return idList;
        }

        private static IEnumerable<string> GetExistingFiles(string filePath, string extension)
        {
            return new DirectoryInfo(filePath).GetFiles($"*{extension}").Select(f => f.Name);
        }

        private void DownloadData(IEnumerable<string> items, string path)
        {
            foreach (var item in items)
            {
                var filePath = Path.Combine(Path.Combine(path, $"{item}.json"));

                if (!File.Exists(filePath))
                {
                    _logger.LogInformation($"{nameof(ITunesService)} looking up {item}.");

                    var data = _apiDataService.LookupSongs(item);
                    var jsonData = JObject.Parse(data);

                    File.WriteAllText(filePath, jsonData.SelectToken("$.results")!.ToString());

                    _logger.LogInformation($"{nameof(ITunesService)} downloaded {item}.");

                    _logger.LogInformation($"{nameof(ITunesService)} waiting {LookupInterval} seconds.");
                    System.Threading.Thread.Sleep(LookupInterval * 1000);
                }
            }
        }

        private void DownloadArtwork(IEnumerable<string> ids, string path)
        {
            var imageFiles = GetExistingFiles(path, ".jpg");
            if (ids != null)
            {
                var collectionFiles = ids.Select(x => $"{x}.jpg");
                var newFiles = collectionFiles.Except(imageFiles).ToList();
                if (newFiles.Count > 0)
                {
                    var releases = new List<Release>();
                    foreach (var newFile in newFiles)
                    {
                        var basePath = Path.Combine(_config.Value.JsonDataBasePath, "itunes", $"{Path.GetFileNameWithoutExtension(newFile)}.json");
                        var release = _releaseService.GetRelease(basePath);
                        releases.Add(release);
                    }
                    _artworkService.DownloadArtwork(releases);

                    _logger.LogInformation($"Downloaded {newFiles.Count} new artwork files");
                }
            }
        }

        private void CheckFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path!.ToLower());
        }
    }
}