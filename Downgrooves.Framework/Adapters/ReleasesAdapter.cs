using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;

namespace Downgrooves.Framework.Adapters
{
    public static class ReleasesAdapter
    {
        public static Release CreateRelease(ITunesCollection collection)
        {
            Release release = new Release();
            release.Id = collection.Id;
            release.Artist = new Artist();
            release.ArtistId = collection.ArtistId;
            release.ArtistName = collection.ArtistName;
            release.ArtistViewUrl = collection.ArtistViewUrl;
            release.ArtworkUrl = "";
            release.BuyUrl = collection.CollectionViewUrl;
            release.CollectionId = collection.Id;
            release.Copyright = collection.Copyright;
            release.Country = collection.Country;
            release.DiscCount = 0;
            release.DiscNumber = 0;
            release.Genre = collection.PrimaryGenreName;
            release.IsOriginal = true;
            release.IsRemix = true;
            release.PreviewUrl = null;
            release.Price = collection.CollectionPrice.Value;
            release.ReleaseDate = collection.ReleaseDate.Value;
            release.Title = collection.CollectionCensoredName;
            release.Tracks = null;
            release.VendorId = 1;
            return release;
        }

        public static Release CreateRelease(ITunesTrack track)
        {
            Release release = new Release();
            release.Id = track.Id;
            release.Artist = new Artist();
            release.ArtistId = track.ArtistId;
            release.ArtistName = track.ArtistName;
            release.ArtistViewUrl = track.ArtistViewUrl;
            release.ArtworkUrl = "";
            release.BuyUrl = "";
            release.CollectionId = track.Id;
            release.Copyright = null;
            release.Country = track.Country;
            release.DiscCount = track.DiscCount;
            release.DiscNumber = track.DiscNumber;
            release.Genre = track.PrimaryGenreName;
            release.IsOriginal = true;
            release.IsRemix = true;
            release.PreviewUrl = track.PreviewUrl;
            release.Price = track.TrackPrice.Value;
            release.ReleaseDate = track.ReleaseDate.Value;
            release.Title = track.TrackCensoredName;
            release.VendorId = 1;
            return release;
        }

        public static ReleaseTrack CreateReleaseTrack(ITunesCollection collection)
        {
            ReleaseTrack releaseTrack = new ReleaseTrack();
            releaseTrack.Id = collection.Id;
            releaseTrack.ArtistName = collection.ArtistName;
            releaseTrack.PreviewUrl = null;
            releaseTrack.Price = collection.CollectionPrice.Value;
            releaseTrack.ReleaseId = collection.Id;
            releaseTrack.Title = collection.CollectionCensoredName;
            releaseTrack.TrackNumber = 0;
            releaseTrack.TrackTimeInMilliseconds = 0;
            return releaseTrack;
        }

        public static ReleaseTrack CreateReleaseTrack(ITunesTrack track)
        {
            ReleaseTrack releaseTrack = new ReleaseTrack();
            releaseTrack.Id = track.Id;
            releaseTrack.ArtistName = track.ArtistName;
            releaseTrack.PreviewUrl = track.PreviewUrl;
            releaseTrack.Price = track.TrackPrice.Value;
            releaseTrack.ReleaseId = track.CollectionId;
            releaseTrack.Title = track.TrackCensoredName;
            releaseTrack.TrackId = track.Id;
            releaseTrack.TrackNumber = track.TrackNumber;
            releaseTrack.TrackTimeInMilliseconds = track.TrackTimeMillis;
            return releaseTrack;
        }

        public static IEnumerable<Release> CreateReleases(IEnumerable<ITunesCollection> collections)
        {
            var releases = new List<Release>();
            foreach (var collectionItem in collections)
            {
                Release release = CreateRelease(collectionItem);
                if (release != null)
                    releases.Add(release);
            }
            return releases;
        }

        public static IEnumerable<Release> CreateReleases(IEnumerable<ITunesTrack> tracks)
        {
            var releases = new List<Release>();
            foreach (var track in tracks)
            {
                Release release = CreateRelease(track);
                if (release != null)
                    releases.Add(release);
            }
            return releases;
        }

        public static IEnumerable<ReleaseTrack> CreateReleaseTracks(IEnumerable<ITunesCollection> collections)
        {
            var releaseTracks = new List<ReleaseTrack>();
            foreach (var collectionItem in collections)
            {
                ReleaseTrack releaseTrack = CreateReleaseTrack(collectionItem);
                if (releaseTrack != null)
                    releaseTracks.Add(releaseTrack);
            }
            return releaseTracks;
        }

        public static IEnumerable<ReleaseTrack> CreateReleaseTracks(IEnumerable<ITunesTrack> tracks)
        {
            var releaseTracks = new List<ReleaseTrack>();
            foreach (var track in tracks)
            {
                ReleaseTrack releaseTrack = CreateReleaseTrack(track);
                if (releaseTrack != null)
                    releaseTracks.Add(releaseTrack);
            }
            return releaseTracks;
        }
    }
}