using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Downgrooves.WorkerService.Config;
using Microsoft.Extensions.Logging;
using Downgrooves.WorkerService.Interfaces;

namespace Downgrooves.WorkerService.Services
{
    public class ReleaseService : IReleaseService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<ReleaseService> _logger;
        private readonly IApiClientService _apiClientService;
        public string ApiUrl { get; }
        public string Token { get; }
        public string ArtworkBasePath { get; }

        public ReleaseService(IOptions<AppConfig> config, ILogger<ReleaseService> logger, IApiClientService apiClientService)
        {
            _appConfig = config.Value;
            ApiUrl = _appConfig.ApiUrl;
            Token = _appConfig.Token;
            ArtworkBasePath = _appConfig.ArtworkBasePath;
            _logger = logger;
            _apiClientService = apiClientService;
        }

        public void AddCollections(IEnumerable<Release> collections)
        {
            IEnumerable<Release> collectionsToAdd = new List<Release>();
            var existingCollections = _apiClientService.GetReleases().Where(x => x.WrapperType == "collection");
            if (existingCollections != null && existingCollections.Count() > 0)
                collectionsToAdd = collections.Where(x => existingCollections.All(y => x.CollectionId != y.CollectionId));
            else
                collectionsToAdd = collections;
            collectionsToAdd = collectionsToAdd.Where(x => x.ReleaseDate > Convert.ToDateTime("1970-01-01")); // do not add pre-release
            var count = _apiClientService.AddNewReleases(collectionsToAdd);
            if (count > 0)
                _logger.LogInformation($"{count} collections added.");
        }

        public void AddTracks(IEnumerable<Release> tracks)
        {
            IEnumerable<Release> tracksToAdd = new List<Release>();
            var existingTracks = _apiClientService.GetReleases().Where(x => x.WrapperType == "track");
            if (existingTracks != null && existingTracks.Count() > 0)
                tracksToAdd = tracks.Where(x => existingTracks.All(y => x.TrackId != y.TrackId));
            else
                tracksToAdd = tracks;
            tracksToAdd = tracksToAdd.Where(x => x.ReleaseDate > Convert.ToDateTime("1970-01-01")); // do not add pre-release
            var count = _apiClientService.AddNewReleases(tracksToAdd);
            if (count > 0)
                _logger.LogInformation($"{count} tracks added.");
        }
    }
}