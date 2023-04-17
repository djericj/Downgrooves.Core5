using Downgrooves.Domain;
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
            var basePath = _config.Value.JsonDataBasePath;
            var existingFiles = GetExistingFiles(Path.Combine(basePath, "iTunes"), ".json").ToList();
            var searchArtistCollections = GetIds(Path.Combine(basePath, "iTunes", "Collections", "Artists"));
            var searchArtistTracks = GetIds(Path.Combine(basePath, "iTunes", "Tracks", "Artists"));
            var newArtistCollections = searchArtistCollections.Except(existingFiles.Select(f => f.Replace(".json",""))).ToList();
            var newArtistTracks = searchArtistTracks.Except(existingFiles.Select(f => f.Replace(".json", ""))).ToList();

            if (newArtistCollections.Count == 0 && newArtistTracks.Count == 0)
            {
                _logger.LogInformation($"{nameof(ProcessWorker)} no new items.");
            }
            else
            {
                DownloadData(newArtistCollections, Path.Combine($"{_config.Value.JsonDataBasePath}", "iTunes"));
                DownloadData(newArtistTracks, Path.Combine($"{_config.Value.JsonDataBasePath}", "iTunes"));
            }
        }

        public void GetArtwork()
        {
            _logger.LogInformation($"{nameof(ProcessWorker)} looking for new artwork.");

            var existingFiles = GetExistingFiles(Path.Combine(_config.Value.JsonDataBasePath, "iTunes"), ".json").ToList();
            var existingArtwork = GetExistingFiles(_artworkBasePath, ".jpg").ToList();
            var newArtworkItems = existingFiles.Select(e => e.Replace(".json", "")).Except(existingArtwork.Select(a => a.Replace(".jpg", ""))).ToList();

            if (newArtworkItems.Count == 0)
                _logger.LogInformation($"{nameof(ProcessWorker)} no new artwork.");
            else
                DownloadArtwork(newArtworkItems.Select(a => a.Replace(".json","")), _artworkBasePath);
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
                        var basePath = Path.Combine(_config.Value.JsonDataBasePath, "iTunes", $"{Path.GetFileNameWithoutExtension(newFile)}.json");
                        var release = _releaseService.GetRelease(basePath);
                        releases.Add(release);
                    }
                    _artworkService.DownloadArtwork(releases);

                    _logger.LogInformation($"Downloaded {newFiles.Count} new artwork files");
                }
            }
        }
    }
}