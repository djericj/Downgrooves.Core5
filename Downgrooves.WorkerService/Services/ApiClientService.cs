﻿using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Downgrooves.WorkerService.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly AppConfig _appConfig;
        public ApiClientService(IOptions<AppConfig> config)
        {
            _appConfig = config.Value;
        }

        public IJEnumerable<JToken> GetItunesJson(string searchTerm)
        {
            string data = null;
            string url = $"https://itunes.apple.com/search/?term={searchTerm}&entity=musicArtist,musicTrack,album,mix,song&media=music&limit=200";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            JObject o = JObject.Parse(data);
            var results = o.SelectTokens("results").Children();
            return results;
        }

        public IJEnumerable<JToken> GetYouTubeVideosJson()
        {
            string data = null;
            var ApiKey = _appConfig.YouTube.ApiKey;
            string url = $"https://youtube.googleapis.com/youtube/v3/playlistItems?part=snippet%2CcontentDetails&maxResults=100&playlistId=PLvrGGNimrTIMSxEt7InO9NK_aUplnK513&key={ApiKey}";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            JObject o = JObject.Parse(data);
            var results = o.SelectTokens("items").Children();
            return results;
        }
    }
}