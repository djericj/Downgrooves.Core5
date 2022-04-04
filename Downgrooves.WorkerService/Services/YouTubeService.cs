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
            var videos = _apiClientService.GetYouTubeVideosJson();
            var existingVideos = GetExistingVideos();
            if (existingVideos != null && existingVideos.Count() > 0)
                videosToAdd = videos.Where(x => existingVideos.All(y => x.Id != y.Id));
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
            var description = $"{video.Title} ({video.Id})";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                index++;
                Console.WriteLine($"Added {description}");
            }
            else
                Console.Error.WriteLine($"Error adding {description}");
        }
    }
}