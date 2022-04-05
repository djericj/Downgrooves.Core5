using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using Downgrooves.Domain.ITunes;
using Downgrooves.Domain.YouTube;

namespace Downgrooves.WorkerService.Services
{
    public class ApiClientService : IApiClientService
    {
        private int index = 0;
        private readonly AppConfig _appConfig;
        private readonly ILogger<ApiClientService> _logger;

        public ApiClientService(IOptions<AppConfig> config, ILogger<ApiClientService> logger)
        {
            _appConfig = config.Value;
            _logger = logger;
        }

        #region Downgrooves Release API

        public int AddNewReleases(IEnumerable<Release> releases)
        {
            foreach (var release in releases)
                AddNewRelease(release);
            return index;
        }

        public void AddNewRelease(Release release)
        {
            try
            {
                var description = $"{release.ArtistName} - {release.CollectionName} ({release.CollectionId})";
                var response = ApiPost("releases", release);
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        public IEnumerable<ITunesExclusion> GetExclusions()
        {
            var response = ApiGet("releases/exclusions");
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IEnumerable<ITunesExclusion>>(response.Content);
            else
                _logger.LogError($"Error getting exclusions.  {response.Content}");
            return null;
        }

        public IEnumerable<Release> GetReleases()
        {
            var response = ApiGet("releases");
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

        public void AddNewITunesItems(IEnumerable<ITunesLookupResultItem> items)
        {
            try
            {
                var response = ApiPost("itunes/range", items);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    index++;
                    _logger.LogInformation($"Added {items.Count()} items.");
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding {items.Count()} items.  {response.Content}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        public void AddNewITunesItem(ITunesLookupResultItem item)
        {
            try
            {
                var description = $"{item.ArtistName} - {item.CollectionName} ({item.CollectionId})";
                var response = ApiPost("itunes", item);
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        public IEnumerable<ITunesLookupResultItem> GetITunesLookupResultItems()
        {
            var response = ApiGet("itunes");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<ITunesLookupResultItem[]>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes items.  {response.Content}");
            return null;
        }

        #endregion Downgrooves iTunes API

        #region Apple iTunes API

        public ITunesLookupResultItem LookupCollectionById(int collectionId)
        {
            string url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";
            var data = GetString(url);
            return JObject.Parse(data).ToObjects<ITunesLookupResultItem>("results").FirstOrDefault();
        }

        public IEnumerable<ITunesLookupResultItem> LookupTracksCollectionById(int collectionId)
        {
            string url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";
            var data = GetString(url);
            return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?.Where(x => x.WrapperType == "track");
        }

        public IEnumerable<ITunesLookupResultItem> LookupCollections(string searchTerm)
        {
            string url = $"https://itunes.apple.com/search/?term={searchTerm}&entity=musicArtist,musicTrack,album,mix,song&media=music&limit=200";
            var data = GetString(url);
            return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?.Where(x => x.WrapperType == "collection");
        }

        public IEnumerable<ITunesLookupResultItem> LookupTracks(string searchTerm)
        {
            string url = $"https://itunes.apple.com/search?term={searchTerm}&entity=song";
            var data = GetString(url);
            return JObject.Parse(data)?.ToObjects<ITunesLookupResultItem>("results")?.Where(x => x.WrapperType == "track");
        }

        #endregion Apple iTunes API

        #region YouTube API

        public IEnumerable<Video> GetYouTubeVideosJson()
        {
            var ApiKey = _appConfig.YouTube.ApiKey;
            string url = $"https://youtube.googleapis.com/youtube/v3/playlistItems?part=snippet%2CcontentDetails&maxResults=100&playlistId=PLvrGGNimrTIMSxEt7InO9NK_aUplnK513&key={ApiKey}";
            var data = GetString(url);
            var results = JsonConvert.DeserializeObject<YouTubeLookupResult>(data);
            var videos = results?.Items?.ToVideos();
            return videos;
        }

        #endregion YouTube API

        private string GetString(string resource)
        {
            using (var webClient = new WebClient())
                return webClient.DownloadString(resource);
        }

        private IRestResponse ApiGet(string resource)
        {
            var client = new RestClient(_appConfig.ApiUrl);
            client.Authenticator = new JwtAuthenticator(_appConfig.Token);
            var request = new RestRequest(resource);
            return client.Get(request);
        }

        private IRestResponse ApiPost(string resource, object value)
        {
            var client = new RestClient(_appConfig.ApiUrl);
            client.Authenticator = new JwtAuthenticator(_appConfig.Token);
            var request = new RestRequest(resource, Method.POST);
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            var json = JsonConvert.SerializeObject(value, settings);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            return client.Post(request);
        }
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