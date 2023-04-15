using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Framework.Adapters;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Downgrooves.WorkerService.Services
{
    public class ReleaseService : ApiService, IReleaseService
    {
        private readonly ILogger<ReleaseService> _logger;
        private readonly IArtistService _artistService;
        private readonly IITunesService _itunesService;

        private IEnumerable<ITunesCollection> _collections;
        private IEnumerable<ITunesTrack> _tracks;

        public ReleaseService(IOptions<AppConfig> config,
            ILogger<ReleaseService> logger,
            IArtistService artistService,
            IITunesService itunesService) : base(config, logger)
        {
            _logger = logger;
            _artistService = artistService;
            _itunesService = itunesService;
        }

        public void ProcessReleases()
        {
            _collections = _itunesService.GetCollections();
            _tracks = _itunesService.GetTracks();

            var existingReleases = GetReleases();

            var newReleases = _collections.Where(c => existingReleases.All(c2 => c2.CollectionId != c.Id));

            _logger.LogInformation($"{nameof(ReleaseService)} {newReleases?.Count()} NEW releases.");

            var addedReleases = Add(ReleasesAdapter.CreateReleases(newReleases));

            _logger.LogInformation($"{nameof(ReleaseService)} {newReleases?.Count()} ADDED releases.");
        }

        public Release Add(Release release)
        {
            return Add("release", release);
        }

        public IEnumerable<Release> Add(IEnumerable<Release> releases)
        {
            return Add("releases", releases);
        }

        public ReleaseTrack AddTrack(ReleaseTrack releaseTrack)
        {
            return Add("release/track", releaseTrack);
        }

        public IEnumerable<ReleaseTrack> AddTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            return Add("release/tracks", releaseTracks);
        }

        public IEnumerable<Release> GetReleases(string artistName = null)
        {
            return Get<IEnumerable<Release>>("releases", new Artist() { Name = artistName });
        }

        public ReleaseTrack GetReleaseTrack(int id)
        {
            return Get<ReleaseTrack>($"/release/track/{id}");
        }

        public Release Update(Release release)
        {
            return Update($"release/{release.Id}", release);
        }

        public ReleaseTrack UpdateTrack(ReleaseTrack releaseTrack)
        {
            return Update($"release/track/{releaseTrack.Id}", releaseTrack);
        }

        public IEnumerable<ReleaseTrack> UpdateTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            return Update($"release/tracks", releaseTracks);
        }
    }
}