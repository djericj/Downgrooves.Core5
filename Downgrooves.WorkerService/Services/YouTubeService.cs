using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Downgrooves.WorkerService.Services
{
    public class YouTubeService : IYouTubeService
    {
        private int index = 0;
        private readonly AppConfig _appConfig;
        private readonly IApiClientService _apiClientService;
        private readonly ILogger<YouTubeService> _logger;

        public string ApiUrl { get; }
        public string Token { get; }

        public YouTubeService(IOptions<AppConfig> config, IApiClientService apiClientService, ILogger<YouTubeService> logger)
        {
            _appConfig = config.Value;
            ApiUrl = _appConfig.ApiUrl;
            Token = _appConfig.Token;
            _apiClientService = apiClientService;
            _logger = logger;
        }

        private IEnumerable<Video> GetExistingVideos()
        {
            var client = new RestClient(ApiUrl);
            client.Authenticator = new JwtAuthenticator(Token);
            var request = new RestRequest("videos");
            var response = client.Get(request);
            var json = response.Content;
            var videos = JsonConvert.DeserializeObject<IEnumerable<Video>>(json);
            return videos;
        }

        public void AddNewVideos()
        {
            IEnumerable<Video> videosToAdd = new List<Video>();
            var results = _apiClientService.GetYouTubeVideosJson();
            var videos = CreateVideos(results);
            var existingVideos = GetExistingVideos();
            if (existingVideos != null && existingVideos.Count() > 0)
                videosToAdd = videos.Where(x => existingVideos.All(y => x.VideoId != y.VideoId));
            else
                videosToAdd = videos;
            var count = AddNewVideos(videosToAdd);
            if (count > 0)
                _logger.LogInformation($"{count} videos added.");
        }

        private int AddNewVideos(IEnumerable<Video> videos)
        {
            foreach (var video in videos)
                AddNewVideo(video);
            return index;
        }

        private void AddNewVideo(Video video)
        {
            var client = new RestClient(ApiUrl);
            client.Authenticator = new JwtAuthenticator(Token);
            var request = new RestRequest("videos", Method.POST);
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            var json = JsonConvert.SerializeObject(video, settings);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = client.Post(request);
            var description = $"{video.Title} ({video.VideoId})";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                index++;
                Console.WriteLine($"Added {description}");
            }
            else
                Console.Error.WriteLine($"Error adding {description}");
        }

        private IEnumerable<Video> CreateVideos(IJEnumerable<JToken> videos)
        {
            var videoList = new List<Video>();
            foreach (var item in videos)
            {
                var video = CreateVideo(item);
                videoList.Add(video);
            }
            return videoList;
        }

        private Video CreateVideo(JToken token)
        {
            var video = new Video();
            var snippet = token.SelectToken("snippet");
            video.Description = snippet.SelectToken("description").ToString();
            video.ETag = token["etag"].ToString();
            video.PublishedAt = Convert.ToDateTime(snippet.SelectToken("publishedAt").ToString());
            video.Thumbnails = GetThumbnails(snippet.SelectToken("thumbnails").Children());
            video.Title = snippet.SelectToken("title").ToString();
            video.VideoId = snippet.SelectToken("resourceId").SelectToken("videoId").ToString();

            return video;
        }

        private ICollection<Thumbnail> GetThumbnails(IEnumerable<JToken> tokens)
        {
            var thumbnails = new List<Thumbnail>();
            foreach (var item in tokens)
            {
                var t = new Thumbnail();
                var child = item.Children().First();
                t.Type = ((JProperty)child.Parent).Name;
                t.Height = Convert.ToInt32(child.SelectToken("height").ToString());
                t.Url = child.SelectToken("url").ToString();
                t.Width = Convert.ToInt32(child.SelectToken("width").ToString());
                thumbnails.Add(t);
            }
            return thumbnails;
        }
    }
}
