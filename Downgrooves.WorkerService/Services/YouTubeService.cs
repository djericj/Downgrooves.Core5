using Downgrooves.Domain;
using Downgrooves.Domain.YouTube;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Extensions;
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
        private readonly ILogger<YouTubeService> _logger;

        public string ApiUrl { get; }
        public string Token { get; }

        public YouTubeService(IOptions<AppConfig> config, ILogger<YouTubeService> logger) : base(config)
        {
            ApiUrl = config.Value.ApiUrl;
            Token = config.Value.Token;
            _logger = logger;
        }

        public async Task<IEnumerable<Video>> GetExistingVideos()
        {
            var response = await ApiGet("videos");
            var json = response.Content;
            var videos = JsonConvert.DeserializeObject<IEnumerable<Video>>(json);
            return videos;
        }

        public async Task<int> AddNewVideos(IEnumerable<Video> videos)
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
                _logger.LogInformation($"Added {description}");
            }
            else
                _logger.LogError($"Error adding {description}");
        }

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
}