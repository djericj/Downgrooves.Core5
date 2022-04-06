using Downgrooves.Domain;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class YouTubeService : ApiBase, IYouTubeService
    {
        private int index = 0;
        private readonly IApiClientService _apiClientService;
        private readonly ILogger<YouTubeService> _logger;

        public string ApiUrl { get; }
        public string Token { get; }

        public YouTubeService(IOptions<AppConfig> config, IApiClientService apiClientService, ILogger<YouTubeService> logger) : base(config)
        {
            ApiUrl = config.Value.ApiUrl;
            Token = config.Value.Token;
            _apiClientService = apiClientService;
            _logger = logger;
        }

        private async Task<IEnumerable<Video>> GetExistingVideos()
        {
            var response = await ApiGet("videos");
            var json = response.Content;
            var videos = JsonConvert.DeserializeObject<IEnumerable<Video>>(json);
            return videos;
        }

        public async Task AddNewVideos()
        {
            IEnumerable<Video> videosToAdd = new List<Video>();
            var videos = await _apiClientService.GetYouTubeVideosJson();
            var existingVideos = await GetExistingVideos();
            if (existingVideos != null && existingVideos.Count() > 0)
                videosToAdd = videos.Where(x => existingVideos.All(y => x.Id != y.Id));
            else
                videosToAdd = videos;
            var count = await AddNewVideos(videosToAdd);
            if (count > 0)
                _logger.LogInformation($"{count} videos added.");
        }

        private async Task<int> AddNewVideos(IEnumerable<Video> videos)
        {
            foreach (var video in videos)
                await AddNewVideo(video);
            return index;
        }

        private async Task AddNewVideo(Video video)
        {
            var response = await ApiPost("video", video);
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