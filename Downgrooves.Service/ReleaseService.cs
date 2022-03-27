using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace Downgrooves.Service
{
    public class ReleaseService : IReleaseService
    {
        private readonly ILogger<IReleaseService> _logger;
        private readonly IITunesService _iTunesService;

        public IEnumerable<Release> Releases { get; set; }

        public ReleaseService(IITunesService iTunesService, ILogger<IReleaseService> logger)
        {
            _iTunesService = iTunesService;
            _logger = logger;
        }

        public async Task<IEnumerable<Release>> Find(Expression<Func<Release, bool>> predicate)
        {
            var collections = await GetCollections();
            return collections.Where(predicate.Compile());
        }

        public async Task<IEnumerable<Release>> GetReleases()
        {
            return await GetCollections();
        }

        public async Task<IEnumerable<Release>> GetReleases(PagingParameters parameters)
        {
            var collections = await GetCollections();
            return collections.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);
        }

        private async Task<IEnumerable<Release>> GetCollections()
        {
            if (Releases == null)
            {
                var iTunesTracks = await _iTunesService.GetTracks();
                var collectionIds = iTunesTracks.GroupBy(x => x.CollectionId).Select(g => g.First()).OrderByDescending(x => x.ReleaseDate);
                var collections = iTunesTracks.Where(x => collectionIds.Contains(x));
                var releases = new List<Release>();
                foreach (var item in collections)
                {
                    var release = ConvertToRelease(item);
                    release.Tracks = GetTracks(item.CollectionId, iTunesTracks).ToList();
                    releases.Add(release);
                }
                Releases = releases.OrderByDescending(x => x.ReleaseDate);
            }
            return Releases;
        }

        private IEnumerable<ITunesTrack> GetTracks(int collectionId, IEnumerable<ITunesTrack> tracks)
        {
            return tracks.Where(x => x.CollectionId == collectionId);
        }

        private Release ConvertToRelease(ITunesTrack track)
        {
            return new Release()
            {
                ArtistName = track.ArtistName,
                ArtistViewUrl = track.ArtistViewUrl,
                ArtworkUrl100 = track.ArtworkUrl100,
                ArtworkUrl30 = track.ArtworkUrl30,
                ArtworkUrl60 = track.ArtworkUrl60,
                CollectionCensoredName = track.CollectionCensoredName,
                CollectionName = track.CollectionName,
                CollectionPrice = track.CollectionPrice,
                CollectionViewUrl = track.CollectionViewUrl,
                Country = track.Country,
                Currency = track.Currency,
                TrackCensoredName = track.TrackCensoredName,
                TrackCount = track.TrackCount,
                TrackName = track.TrackName,
                TrackPrice = track.TrackPrice,
                TrackViewUrl = track.TrackViewUrl,
                Id = track.CollectionId,
                PreviewUrl = track.PreviewUrl,
                PrimaryGenreName = track.PrimaryGenreName,
                ReleaseDate = track.ReleaseDate,
            };
        }
    }
}
