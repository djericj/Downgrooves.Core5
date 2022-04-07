using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Domain.YouTube;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Downgrooves.WorkerService.Extensions
{
    public static class ITunesLoaderExtensions
    {
        public static IList<T> ToObjects<T>(this JObject obj, string path)
        {
            var jArray = (JArray)obj[path];
            return jArray.ToObject<IList<T>>();
        }

        public static IList<Video> ToVideos(this IEnumerable<YouTubeVideo> youTubeVideos)
        {
            var videoList = new List<Video>();
            foreach (var youTubeVideo in youTubeVideos)
                videoList.Add(youTubeVideo.ToVideo());
            return videoList;
        }

        public static Video ToVideo(this YouTubeVideo youTubeVideo)
        {
            return new Video()
            {
                Description = youTubeVideo.Snippet.Description,
                ETag = youTubeVideo.ETag,
                SourceSystemId = youTubeVideo.Id,
                PublishedAt = youTubeVideo.Snippet.PublishedAt,
                Thumbnails = youTubeVideo.Snippet.Thumbnails.ToThumbnails(),
                Title = youTubeVideo.Snippet?.Title,
            };
        }

        public static IList<Thumbnail> ToThumbnails(this Thumbnails youTubeThumbnails)
        {
            var thumbnails = new List<Thumbnail>();

            thumbnails.Add(youTubeThumbnails.Standard?.ToThumbnail("standard"));
            thumbnails.Add(youTubeThumbnails.Default?.ToThumbnail("default"));
            thumbnails.Add(youTubeThumbnails.Medium?.ToThumbnail("medium"));
            thumbnails.Add(youTubeThumbnails.High?.ToThumbnail("high"));
            thumbnails.Add(youTubeThumbnails.MaxResolution?.ToThumbnail("maxres"));

            thumbnails = thumbnails.Where(x => x != null).ToList();

            return thumbnails;
        }

        public static Thumbnail ToThumbnail(this ThumbnailImage youTubeThumbnail, string type)
        {
            return new Thumbnail()
            {
                Height = youTubeThumbnail.Height,
                Width = youTubeThumbnail.Width,
                Url = youTubeThumbnail.Url,
                Type = type
            };
        }

        public static Release ToRelease(this ITunesCollection item, Artist artist)
        {
            return new Release()
            {
                ArtistId = artist.ArtistId,
                //Artist = artist,
                ArtistName = item.ArtistName,
                ArtistViewUrl = item.ArtistViewUrl,
                ArtworkUrl = item.CollectionId + ".jpg",
                BuyUrl = item.CollectionViewUrl,
                Copyright = item.Copyright,
                Country = item.Country,
                Genre = item.PrimaryGenreName,
                IsOriginal = item.WrapperType == "collection",
                IsRemix = item.WrapperType == "track",
                ReleaseDate = item.ReleaseDate,
                SourceSystemId = item.CollectionId,
                Price = item.CollectionPrice,
                Title = item.CollectionCensoredName,
            };
        }

        public static Release ToRelease(this ITunesLookupResultItem item)
        {
            return new Release()
            {
                ArtistName = item.ArtistName,
                ArtistViewUrl = item.ArtistViewUrl,
                ArtworkUrl = item.CollectionId + ".jpg",
                BuyUrl = item.CollectionViewUrl,
                Copyright = item.Copyright,
                Country = item.Country,
                DiscCount = item.DiscCount,
                DiscNumber = item.DiscNumber,
                Genre = item.PrimaryGenreName,
                IsOriginal = item.WrapperType == "collection",
                IsRemix = item.WrapperType == "track",
                PreviewUrl = item.PreviewUrl,
                ReleaseDate = item.ReleaseDate,
                SourceSystemId = item.CollectionId,
                Price = item.CollectionPrice,
                Title = item.CollectionCensoredName,
                VendorId = 1
            };
        }

        public static IList<Release> ToReleases(this IEnumerable<ITunesLookupResultItem> items)
        {
            var releases = new List<Release>();
            foreach (var item in items)
                releases.Add(item.ToRelease());
            return releases;
        }

        public static ReleaseTrack ToReleaseTrack(this ITunesTrack item, Release release)
        {
            return new ReleaseTrack()
            {
                ArtistName = item.ArtistName,
                Price = item.TrackPrice,
                Title = item.TrackCensoredName,
                PreviewUrl = item.PreviewUrl,
                TrackNumber = item.TrackNumber,
                TrackTimeInMilliseconds = item.TrackTimeMillis,
                SourceSystemId = item.TrackId,
                ReleaseId = release.Id
            };
        }

        public static IEnumerable<ReleaseTrack> ToReleaseTracks(this IEnumerable<ITunesTrack> items, Release release)
        {
            var tracks = new List<ReleaseTrack>();
            foreach (var item in items)
                tracks.Add(item.ToReleaseTrack(release));
            return tracks;
        }

        public static ReleaseTrack ToReleaseTrack(this ITunesLookupResultItem item, Release release)
        {
            return new ReleaseTrack()
            {
                ArtistName = item.ArtistName,
                Price = item.TrackPrice,
                Title = item.TrackCensoredName,
                PreviewUrl = item.PreviewUrl,
                TrackNumber = item.TrackNumber,
                TrackTimeInMilliseconds = item.TrackTimeMillis,
                SourceSystemId = item.TrackId,
                ReleaseId = release.Id
            };
        }

        public static IList<ReleaseTrack> ToReleaseTracks(this IEnumerable<ITunesLookupResultItem> items, Release release)
        {
            var tracks = new List<ReleaseTrack>();
            foreach (var item in items)
                tracks.Add(item.ToReleaseTrack(release));
            return tracks;
        }

        public static IList<ITunesCollection> ToITunesCollections(this IEnumerable<ITunesLookupResultItem> items, Artist artist)
        {
            var collections = new List<ITunesCollection>();
            foreach (var item in items)
            {
                collections.Add(new ITunesCollection()
                {
                    ArtistId = artist.ArtistId,
                    ArtistName = item.ArtistName,
                    ArtistViewUrl = item.ArtistViewUrl,
                    ArtworkUrl100 = item.ArtworkUrl100,
                    ArtworkUrl60 = item.ArtworkUrl60,
                    CollectionCensoredName = item.CollectionCensoredName,
                    CollectionExplicitness = item.CollectionExplicitness,
                    CollectionId = item.CollectionId,
                    CollectionName = item.CollectionName,
                    CollectionPrice = item.CollectionPrice,
                    CollectionType = item.CollectionType,
                    CollectionViewUrl = item.CollectionViewUrl,
                    Copyright = item.Copyright,
                    Country = item.Country,
                    Currency = item.Currency,
                    TrackCount = item.TrackCount,
                    WrapperType = item.WrapperType,
                    PrimaryGenreName = item.PrimaryGenreName,
                    ReleaseDate = item.ReleaseDate,
                    SourceArtistId = item.ArtistId
                });
            }
            return collections;
        }

        public static IList<ITunesTrack> ToITunesTracks(this IEnumerable<ITunesLookupResultItem> items, Artist artist)
        {
            var tracks = new List<ITunesTrack>();
            foreach (var item in items)
            {
                tracks.Add(new ITunesTrack()
                {
                    ArtistId = artist.ArtistId,
                    ArtistName = item.ArtistName,
                    ArtistViewUrl = item.ArtistViewUrl,
                    ArtworkUrl100 = item.ArtworkUrl100,
                    ArtworkUrl30 = item.ArtworkUrl30,
                    ArtworkUrl60 = item.ArtworkUrl60,
                    CollectionCensoredName = item.CollectionCensoredName,
                    CollectionExplicitness = item.CollectionExplicitness,
                    CollectionId = item.CollectionId,
                    CollectionName = item.CollectionName,
                    CollectionPrice = item.CollectionPrice,
                    CollectionViewUrl = item.CollectionViewUrl,
                    Country = item.Country,
                    Currency = item.Currency,
                    DiscCount = item.DiscCount,
                    TrackCensoredName = item.TrackCensoredName,
                    DiscNumber = item.DiscNumber,
                    IsStreamable = item.IsStreamable,
                    Kind = item.Kind,
                    PreviewUrl = item.PreviewUrl,
                    PrimaryGenreName = item.PrimaryGenreName,
                    ReleaseDate = item.ReleaseDate,
                    SourceArtistId = item.ArtistId,
                    TrackCount = item.TrackCount,
                    TrackExplicitness = item.TrackExplicitness,
                    TrackId = item.TrackId,
                    TrackName = item.TrackName,
                    TrackNumber = item.TrackNumber,
                    TrackPrice = item.TrackPrice,
                    TrackTimeMillis = item.TrackTimeMillis,
                    TrackViewUrl = item.TrackViewUrl,
                    WrapperType = item.WrapperType,
                });
            }
            return tracks;
        }
    }
}