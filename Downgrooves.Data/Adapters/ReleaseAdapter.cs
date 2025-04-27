using Downgrooves.Data.Types;
using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;

namespace Downgrooves.Data.Adapters
{
    public static class ReleaseAdapter
    {
        private static IEnumerable<Artist>? _artists = null;

        public static IEnumerable<Release> CreateReleases(IEnumerable<ITunesCollection> collections, IEnumerable<ITunesTrack>? tracks = null)
        {
            var releases = new List<Release>();

            foreach (var collection in collections)
            {
                var releaseTracks = CreateTracks((tracks ?? Array.Empty<ITunesTrack>()).Where(t => t.CollectionId == collection.Id).ToList()) as ICollection<ReleaseTrack>;
                releases.Add(CreateRelease(collection, releaseTracks));
            }

            return releases;
        }

        public static Release CreateRelease(ITunesCollection collection, ICollection<ReleaseTrack>? tracks = null)
        {
            var release = new Release
            {
                ArtistId = collection.ArtistId.GetValueOrDefault(),
                ArtistName = collection.ArtistName,
                ArtistViewUrl = collection.ArtistViewUrl,
                ArtworkUrl = $"{collection.Id}.{FileTypes.Jpg}",
                BuyUrl = collection.CollectionViewUrl,
                CollectionId = collection.Id.GetValueOrDefault(),
                Copyright = collection.Copyright,
                Country = collection.Country,
                Genre = collection.PrimaryGenreName,
                Id = collection.Id.GetValueOrDefault(),
                IsOriginal = false,
                IsRemix = tracks?.Any(t => t.Title.Contains(collection.ArtistName, StringComparison.OrdinalIgnoreCase)) ?? false,
                Price = collection.CollectionPrice.GetValueOrDefault(),
                ReleaseDate = collection.ReleaseDate.GetValueOrDefault(),
                Title = collection.CollectionCensoredName,
                Tracks = tracks,
                VendorId = 1
            };

            release.Artist = GetArtist(release);
            if (release.Artist != null)
            {
                release.IsOriginal = release.ArtistName.Contains(release.Artist?.Name!, StringComparison.OrdinalIgnoreCase);
                if (release.Tracks != null)
                    release.IsRemix = release.Tracks.Any(t =>
                        t.Title.Contains(release.Artist?.Name!, StringComparison.OrdinalIgnoreCase));
            }

            return release;
        }

        public static IEnumerable<ReleaseTrack> CreateTracks(IEnumerable<ITunesTrack> items)
        {
            var tracks = new List<ReleaseTrack>();

            tracks.AddRange(items.Select(CreateTrack));

            return tracks.OrderBy(t => t.TrackNumber).ToList();
        }

        public static ReleaseTrack CreateTrack(ITunesTrack item)
        {
            return new ReleaseTrack
            {
                ArtistName = item.ArtistName,
                Id = item.Id.GetValueOrDefault(),
                TrackNumber = item.TrackNumber.GetValueOrDefault(),
                PreviewUrl = item.PreviewUrl,
                Price = item.TrackPrice.GetValueOrDefault(),
                TrackId = item.Id.GetValueOrDefault(),
                TrackTimeInMilliseconds = item.TrackTimeMillis.GetValueOrDefault(),
                ReleaseId = item.CollectionId.GetValueOrDefault(),
                Title = item.TrackCensoredName
            };
        }

        public static IEnumerable<ITunesCollection> CreateCollections(IEnumerable<ITunesLookupResultItem>? items)
        {
            var collections = new List<ITunesCollection>();

            if (items == null)
                return collections;

            collections.AddRange(items.Select(CreateCollection));

            return collections;
        }

        public static ITunesCollection CreateCollection(ITunesLookupResultItem item)
        {
            return new ITunesCollection
            {
                ArtistId = item.ArtistId,
                ArtistName = item.ArtistName,
                ArtistViewUrl = item.ArtistViewUrl,
                ArtworkUrl100 = item.ArtworkUrl100,
                ArtworkUrl60 = item.ArtworkUrl60,
                CollectionCensoredName = item.CollectionCensoredName,
                CollectionExplicitness = item.CollectionExplicitness,
                CollectionPrice = item.CollectionPrice,
                CollectionViewUrl = item.CollectionViewUrl,
                Copyright = item.Copyright,
                Country = item.Country,
                Currency = item.Currency,
                TrackCount = item.TrackCount,
                WrapperType = item.WrapperType,
                Id = item.CollectionId,
                PrimaryGenreName = item.PrimaryGenreName,
                ReleaseDate = item.ReleaseDate
            };
        }

        public static IEnumerable<ITunesTrack> CreateTracks(IEnumerable<ITunesLookupResultItem>? items)
        {
            var tracks = new List<ITunesTrack>();

            if (items == null)
                return tracks;

            tracks.AddRange(items.Select(CreateTrack));

            return tracks;
        }

        public static ITunesTrack CreateTrack(ITunesLookupResultItem item)
        {
            return new ITunesTrack
            {
                ArtistId = item.ArtistId,
                ArtistName = item.ArtistName,
                ArtistViewUrl = item.ArtistViewUrl,
                ArtworkUrl100 = item.ArtworkUrl100,
                ArtworkUrl60 = item.ArtworkUrl60,
                CollectionCensoredName = item.CollectionCensoredName,
                CollectionExplicitness = item.CollectionExplicitness,
                CollectionId = item.CollectionId,
                CollectionPrice = item.CollectionPrice,
                CollectionViewUrl = item.CollectionViewUrl,
                Country = item.Country,
                Currency = item.Currency,
                DiscCount = item.DiscCount,
                TrackCensoredName = item.TrackCensoredName,
                TrackCount = item.TrackCount,
                TrackExplicitness = item.TrackExplicitness,
                TrackName = item.TrackName,
                TrackNumber = item.TrackNumber.GetValueOrDefault(),
                TrackPrice = item.TrackPrice,
                TrackTimeMillis = item.TrackTimeMillis.GetValueOrDefault(),
                TrackViewUrl = item.TrackViewUrl,
                WrapperType = item.WrapperType,
                DiscNumber = item.DiscNumber,
                Id = item.TrackId,
                IsStreamable = item.IsStreamable,
                Kind = item.Kind,
                PreviewUrl = item.PreviewUrl,
                PrimaryGenreName = item.PrimaryGenreName,
                ReleaseDate = item.ReleaseDate
            };
        }

        private static Artist? GetArtist(Release release)
        {
            _artists ??= GetArtists();

            return _artists.FirstOrDefault(artist => release.ArtistName.Contains(artist.Name, StringComparison.OrdinalIgnoreCase) || 
                                                     release.Tracks.Any(t => t.Title.Contains(artist.Name, StringComparison.OrdinalIgnoreCase)));
        }

        private static IEnumerable<Artist> GetArtists()
        {
            return new List<Artist>
            {
                new() { Id = 1, Name = ArtistNames.Downgrooves },
                new() { Id = 2, Name = ArtistNames.EricRylos },
                new() { Id = 3, Name = ArtistNames.Evotone }
            };
        }

        public static Artist? GetArtist(string artistName)
        {
            if (string.Compare(artistName, ArtistNames.Downgrooves, StringComparison.OrdinalIgnoreCase) == 0)
                return new Artist { Id = 1, Name = ArtistNames.Downgrooves };

            if (string.Compare(artistName, ArtistNames.EricRylos, StringComparison.OrdinalIgnoreCase) == 0)
                return new Artist { Id = 2, Name = ArtistNames.EricRylos };

            if (string.Compare(artistName, ArtistNames.Evotone, StringComparison.OrdinalIgnoreCase) == 0)
                return new Artist { Id = 3, Name = ArtistNames.Evotone };

            return null;
        }
    }
}