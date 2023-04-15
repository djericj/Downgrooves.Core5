using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Framework.Adapters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downgrooves.Data
{
    public class ReleasesDao
    {
        private readonly IEnumerable<Release>? _releases;

        private IEnumerable<ITunesCollection> _collections;
        private IEnumerable<ITunesTrack> _tracks;

        public IEnumerable<Release> GetReleases(string artist)
        {
            _tracks = GetTracks(artist);
            _collections = GetCollections(artist);

            var releases = CreateReleases(_collections);

            return releases;
        }

        private IEnumerable<ITunesCollection> GetCollections(string artist) 
        {
            var lookup = JsonConvert.DeserializeObject<ITunesLookupResult>(File.ReadAllText($"D:\\code\\Downgrooves\\Downgrooves.Core5\\Json\\iTunes\\Collections\\{artist}.json"));

            IEnumerable<ITunesCollection> collections = CreateCollections(lookup.Results);

            return collections;
        }

        private IEnumerable<ITunesTrack> GetTracks(string artist)
        {
            var lookup = JsonConvert.DeserializeObject<ITunesLookupResult>(File.ReadAllText($"D:\\code\\Downgrooves\\Downgrooves.Core5\\Json\\iTunes\\Tracks\\{artist}.json"));

            IEnumerable<ITunesTrack> tracks = CreateTracks(lookup.Results);

            return tracks;
        }

        private IEnumerable<ITunesTrack> GetTracks(int collectionId)
        {
            return _tracks.Where(t => t.CollectionId == collectionId);
        }

        private IEnumerable<Release> CreateReleases(IEnumerable<ITunesCollection> collections)
        {
            var releases = new List<Release>();

            foreach (var collection in collections)
                releases.Add(CreateRelease(collection));

            return releases;
        }

        private Release CreateRelease(ITunesCollection collection)
        {
            return new Release
            {
                ArtistName = collection.ArtistName,
                ArtistViewUrl = collection.ArtistViewUrl,
                ArtworkUrl = collection.ArtworkUrl100,
                BuyUrl = collection.CollectionViewUrl,
                CollectionId = collection.Id,
                Copyright = collection.Copyright,
                Country = collection.Copyright,
                Genre = collection.PrimaryGenreName,
                Id = collection.Id,
                Price = collection.CollectionPrice.GetValueOrDefault(),
                ReleaseDate = collection.ReleaseDate.GetValueOrDefault(),
                Title = collection.CollectionCensoredName,
                Tracks = CreateTracks(_tracks.Where(t => t.CollectionId == collection.Id)) as ICollection<ReleaseTrack>,
                VendorId = 1
            };
        }

        private static IEnumerable<ReleaseTrack> CreateTracks(IEnumerable<ITunesTrack> items)
        {
            var tracks = new List<ReleaseTrack>();

            foreach (var item in items)
                tracks.Add(CreateTrack(item));

            return tracks;
        }

        private static ReleaseTrack CreateTrack(ITunesTrack item)
        {
            return new ReleaseTrack
            {
                ArtistName = item.ArtistName,
                TrackNumber = item.TrackNumber,
                PreviewUrl = item.PreviewUrl,
                Price = item.TrackPrice.GetValueOrDefault(),
                TrackId = item.Id,
                TrackTimeInMilliseconds = item.TrackTimeMillis,
                ReleaseId = item.CollectionId,
                Title = item.TrackCensoredName
            };
        }

        private IEnumerable<ITunesCollection> CreateCollections(IEnumerable<ITunesLookupResultItem> items)
        {            
            var collections = new List<ITunesCollection>();
            
            if (items != null)
            {
                foreach (var item in items)
                    collections.Add(CreateCollection(item));
            }

            return collections;
        }

        private static ITunesCollection CreateCollection(ITunesLookupResultItem item)
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

        private IEnumerable<ITunesTrack> CreateTracks(IEnumerable<ITunesLookupResultItem> items)
        {
            var tracks = new List<ITunesTrack>();

            foreach (var item in items)
                tracks.Add(CreateTrack(item));

            return tracks;
        }

        private static ITunesTrack CreateTrack(ITunesLookupResultItem item)
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
                TrackNumber = item.TrackNumber,
                TrackPrice = item.TrackPrice,
                TrackTimeMillis = item.TrackTimeMillis,
                TrackViewUrl = item.TrackViewUrl,
                WrapperType = item.WrapperType,
                DiscNumber = item.DiscNumber,
                Id = item.Id,
                IsStreamable = item.IsStreamable,
                Kind = item.Kind,
                PreviewUrl = item.PreviewUrl,
                PrimaryGenreName = item.PrimaryGenreName,
                ReleaseDate = item.ReleaseDate
            };
        }
    }
}
