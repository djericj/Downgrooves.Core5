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
                var client = new RestClient(_appConfig.ApiUrl);
                client.Authenticator = new JwtAuthenticator(_appConfig.Token);
                var request = new RestRequest("releases", Method.POST);
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var json = JsonConvert.SerializeObject(release, settings);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                var description = $"{release.ArtistName} - {release.CollectionName} ({release.CollectionId})";
                var response = client.Post(request);
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
                    _logger.LogError($"Error adding {description}.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
        }

        public IEnumerable<ITunesExclusion> GetExclusions()
        {
            var client = new RestClient(_appConfig.ApiUrl);
            client.Authenticator = new JwtAuthenticator(_appConfig.Token);
            var request = new RestRequest("releases/exclusions");
            var response = client.Get(request);
            if (response.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IEnumerable<ITunesExclusion>>(response.Content);
            else
                _logger.LogError($"Error getting existing releases.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");
            return null;
        }

        public IEnumerable<Release> GetReleases()
        {
            IEnumerable<Release> collections = null;
            var client = new RestClient(_appConfig.ApiUrl);
            client.Authenticator = new JwtAuthenticator(_appConfig.Token);
            var request = new RestRequest("releases");
            var response = client.Get(request);
            if (response.StatusCode == HttpStatusCode.OK)
                if (response.Content != null && response.Content != "[]")
                    return JsonConvert.DeserializeObject<Release[]>(response.Content);
                else
                    return null;
            else
                _logger.LogError($"Error getting existing releases.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");
            return collections;
        }

        public Release LookupCollectionById(int collectionId)
        {
            string data = null;
            string url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            return JObject.Parse(data).ToObjects<Release>("results").FirstOrDefault();
        }

        public IEnumerable<Release> LookupTracksCollectionById(int collectionId)
        {
            string data = null;
            string url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            return JObject.Parse(data)?.ToObjects<Release>("results")?.Where(x => x.WrapperType == "track");
        }

        public IEnumerable<Release> LookupCollections(string searchTerm)
        {
            string data = null;
            string url = $"https://itunes.apple.com/search/?term={searchTerm}&entity=musicArtist,musicTrack,album,mix,song&media=music&limit=200";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            return JObject.Parse(data)?.ToObjects<Release>("results")?.Where(x => x.WrapperType == "collection");
        }

        public IEnumerable<Release> LookupTracks(string searchTerm)
        {
            string data = null;
            string url = $"https://itunes.apple.com/search?term={searchTerm}&entity=song";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            return JObject.Parse(data)?.ToObjects<Release>("results")?.Where(x => x.WrapperType == "track");
        }

        public IEnumerable<Video> GetYouTubeVideosJson()
        {
            string data = null;
            var ApiKey = _appConfig.YouTube.ApiKey;
            string url = $"https://youtube.googleapis.com/youtube/v3/playlistItems?part=snippet%2CcontentDetails&maxResults=100&playlistId=PLvrGGNimrTIMSxEt7InO9NK_aUplnK513&key={ApiKey}";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            return JObject.Parse(data)?.ToObjects<Video>("items");
        }
    }

    public static class JObjectExtensions
    {
        public static IList<T> ToObjects<T>(this JObject obj, string path)
        {
            var jArray = (JArray)obj[path];
            return jArray.ToObject<IList<T>>();
        }
    }
}