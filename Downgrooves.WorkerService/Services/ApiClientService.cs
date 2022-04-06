using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using Downgrooves.Domain.ITunes;
using Downgrooves.Domain.YouTube;
using Downgrooves.WorkerService.Base;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ApiClientService : ApiBase, IApiClientService
    {
        private int index = 0;
        private readonly ILogger<ApiClientService> _logger;

        public ApiClientService(IOptions<AppConfig> config, ILogger<ApiClientService> logger) : base(config)
        {
            _logger = logger;
        }

        #region Downgrooves Release API

        public async Task<int> AddNewReleases(IEnumerable<Release> releases)
        {
            index = 0;
            foreach (var release in releases)
                await AddNewRelease(release);
            return index;
        }

        public async Task<Release> AddNewRelease(Release release)
        {
            try
            {
                var description = $"{release.ArtistName} - {release.Title}";
                var response = await ApiPost("release", release);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {description}");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {description}.  {response.Content}");
                return JsonConvert.DeserializeObject<Release>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<int> AddNewReleaseTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            index = 0;
            foreach (var track in releaseTracks)
                await AddNewReleaseTrack(track);
            return index;
        }

        public async Task<ReleaseTrack> AddNewReleaseTrack(ReleaseTrack releaseTrack)
        {
            try
            {
                var description = $"{releaseTrack.ArtistName} - {releaseTrack.Title}";
                var response = await ApiPost("release/track", releaseTrack);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {description}");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {description}.  {response.Content}");
                return JsonConvert.DeserializeObject<ReleaseTrack>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<ITunesExclusion>> GetExclusions()
        {
            var response = await ApiGet("itunes/exclusions");
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IEnumerable<ITunesExclusion>>(response.Content);
            else
                _logger.LogError($"Error getting exclusions.  {response.Content}");
            return null;
        }

        public async Task<IEnumerable<Release>> GetReleases()
        {
            var response = await ApiGet($"releases");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<Release[]>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing releases.  {response.Content}");
            return null;
        }

        #endregion Downgrooves Release API

        #region Downgrooves iTunes API

        public async Task<IEnumerable<ITunesCollection>> AddNewCollections(IEnumerable<ITunesCollection> items)
        {
            try
            {
                var response = await ApiPost("itunes/collections", items);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {items.Count()} collections.");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {items.Count()} collections.  {response.Content}");
                return JsonConvert.DeserializeObject<IEnumerable<ITunesCollection>>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<ITunesCollection> AddNewCollection(ITunesCollection item)
        {
            try
            {
                var description = $"{item.ArtistName} - {item.CollectionName} ({item.CollectionId})";
                var response = await ApiPost("itunes/collection", item);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {description}");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {description}.  {response.Content}");
                return JsonConvert.DeserializeObject<ITunesCollection>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections()
        {
            var response = await ApiGet("itunes/collections");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<ITunesCollection[]>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes collections.  {response.Content}");
            return null;
        }

        public async Task<IEnumerable<ITunesTrack>> AddNewTracks(IEnumerable<ITunesTrack> items)
        {
            try
            {
                var response = await ApiPost("itunes/tracks", items);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {items.Count()} tracks.");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {items.Count()} items.  {response.Content}");
                return JsonConvert.DeserializeObject<IEnumerable<ITunesTrack>>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<ITunesTrack> AddNewTrack(ITunesTrack item)
        {
            try
            {
                var description = $"{item.ArtistName} - {item.CollectionName} ({item.CollectionId})";
                var response = await ApiPost("itunes/track", item);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {description}");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {description}.  {response.Content}");
                return JsonConvert.DeserializeObject<ITunesTrack>(response.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks()
        {
            var response = await ApiGet("itunes/tracks");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<ITunesTrack[]>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes tracks.  {response.Content}");
            return null;
        }

        #endregion Downgrooves iTunes API

        #region Apple iTunes API

        public async Task<ITunesLookupResultItem> LookupCollectionById(int collectionId)
        {
            string url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";
            var data = await GetString(url);
            return JObject.Parse(data).ToObjects<ITunesLookupResultItem>("results").FirstOrDefault();
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> LookupTracksCollectionById(int collectionId)
        {
            string url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";
            var data = await GetString(url);
            return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?.Where(x => x.WrapperType == "track");
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> LookupCollections(string searchTerm)
        {
            string url = $"https://itunes.apple.com/search/?term={searchTerm}&entity=musicArtist,musicTrack,album,mix,song&media=music&limit=200";
            var data = await GetString(url);
            return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?.Where(x => x.WrapperType == "collection");
        }

        public async Task<IEnumerable<ITunesLookupResultItem>> LookupTracks(string searchTerm)
        {
            string url = $"https://itunes.apple.com/search?term={searchTerm}&entity=song";
            var data = await GetString(url);
            return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?.Where(x => x.WrapperType == "track");
        }

        #endregion Apple iTunes API

        #region YouTube API

        public async Task<IEnumerable<Video>> GetYouTubeVideosJson()
        {
            var ApiKey = _appConfig.YouTube.ApiKey;
            string url = $"https://youtube.googleapis.com/youtube/v3/playlistItems?part=snippet%2CcontentDetails&maxResults=100&playlistId=PLvrGGNimrTIMSxEt7InO9NK_aUplnK513&key={ApiKey}";
            var data = await GetString(url);
            var results = JsonConvert.DeserializeObject<YouTubeLookupResult>(data);
            var videos = results?.Items?.ToVideos();
            return videos;
        }

        #endregion YouTube API
    }

    public static class ApiClientServiceExtensions
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
                Id = youTubeVideo.Id,
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
    }
}