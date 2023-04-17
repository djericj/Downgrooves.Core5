using System;
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

        public void GetData()
        {
            var existingCollections = GetExistingCollections().ToList();
            var existingTracks = GetExistingTracks().ToList();
            var basePath = _config.Value.JsonDataBasePath;
            var searchCollections = GetIds(Path.Combine(basePath, "iTunes", "Collections", "Artists"));
            var searchTracks = GetIds(Path.Combine(basePath, "iTunes", "Tracks", "Artists"));

            var newCollections = searchCollections.Except(existingCollections).ToList();
            var newTracks = searchTracks.Except(existingTracks).ToList();

            DownloadData(newCollections, Path.Combine($"{_config.Value.JsonDataBasePath}", "iTunes"));
            DownloadData(newTracks, Path.Combine($"{_config.Value.JsonDataBasePath}", "iTunes"));

            _logger.LogInformation($"{nameof(ProcessWorker)} getting any new artwork.");

            DownloadArtwork("collections", existingCollections.Select(x => x[..x.LastIndexOf(".", StringComparison.Ordinal)]).ToList());
            DownloadArtwork("tracks", existingTracks.Select(x => x[..x.LastIndexOf(".", StringComparison.Ordinal)]).ToList());
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

        #region iTunes JSON

        private IEnumerable<string> GetExistingCollections()
        {
            return GetExistingFiles(Path.Combine($"{_config.Value.JsonDataBasePath}", "iTunes", "Collections"));
        }

        private IEnumerable<string> GetExistingTracks()
        {
            return GetExistingFiles(Path.Combine($"{_config.Value.JsonDataBasePath}", "iTunes", "Tracks"));
        }

        private static IEnumerable<string> GetExistingFiles(string filePath)
        {
            var directoryInfo = new DirectoryInfo(filePath);

            return directoryInfo.GetFiles("*.json").Select(f => f.Name);
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

        private void DownloadArtwork(string type, IEnumerable<string> ids)
        {
            var imageFiles = GetImageFiles(Path.Combine(_artworkBasePath, type));
            if (ids != null)
            {
                var collectionFiles = ids.Select(x => $"{x}.jpg");
                var newFiles = collectionFiles.Except(imageFiles).ToList();
                if (newFiles.Count > 0)
                {
                    var releases = new List<Release>();
                    foreach (var newFile in newFiles)
                    {
                        var path = Path.Combine(_config.Value.JsonDataBasePath, "iTunes", type, $"{Path.GetFileNameWithoutExtension(newFile)}.json");
                        var release = _releaseService.GetRelease(path, type);
                        releases.Add(release);
                    }
                    _artworkService.DownloadArtwork(releases);

                    _logger.LogInformation($"Downloaded {newFiles.Count} new artwork files");
                }
            }
        }

        private static IEnumerable<string> GetImageFiles(string path)
        {
            var dir = new DirectoryInfo(path);
            return dir.GetFiles("*.jpg").ToList().Select(x => x.Name);
        }

        #endregion iTunes JSON
       
    }
}