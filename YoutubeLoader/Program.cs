using Downgrooves.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using YoutubeLoader.Interfaces;

namespace YoutubeLoader
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IConfigurationReader ConfigurationReader { get; set; }
        public static string ApiKey { get; set; }
        public static string ApiUrl { get; set; }
        public static string Token { get; set; }

        private static int index = 0;

        private static void Main(string[] args)
        {
            AppConfigurationBuilder();

            IEnumerable<Video> videosToAdd = new List<Video>();
            var json = GetVideosJson();
            JObject o = JObject.Parse(json);
            IJEnumerable<JToken> items = o.SelectTokens("items").Children();
            var videos = CreateVideos(items);
            var existingVideos = GetExistingVideos();
            if (existingVideos != null)
                videosToAdd = videos.Where(x => existingVideos.All(y => x.VideoId != y.VideoId));
            else
                videosToAdd = videos;
            AddNewVideos(videosToAdd);
            Console.WriteLine($"{index} videos added.");
        }

        public static string GetVideosJson()
        {
            string data = null;
            string url = $"https://youtube.googleapis.com/youtube/v3/playlistItems?part=snippet%2CcontentDetails&maxResults=100&playlistId=PLvrGGNimrTIMSxEt7InO9NK_aUplnK513&key={ApiKey}";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            return data;
        }

        private static IEnumerable<Video> GetExistingVideos()
        {
            var client = new RestClient(ApiUrl);
            client.Authenticator = new JwtAuthenticator(Token);
            var request = new RestRequest("videos");
            var response = client.Get(request);
            var json = response.Content;
            var videos = JsonConvert.DeserializeObject<IEnumerable<Video>>(json);
            return videos;
        }

        private static void AddNewVideos(IEnumerable<Video> videos)
        {
            foreach (var video in videos)
                AddNewVideo(video);
        }

        private static void AddNewVideo(Video video)
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

        private static IEnumerable<Video> CreateVideos(IJEnumerable<JToken> videos)
        {
            var videoList = new List<Video>();
            foreach (var item in videos)
            {
                var video = CreateVideo(item);
                videoList.Add(video);
            }
            return videoList;
        }

        private static Video CreateVideo(JToken token)
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

        private static ICollection<Thumbnail> GetThumbnails(IEnumerable<JToken> tokens)
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

        private static void AppConfigurationBuilder()
        {
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            var builder = new ConfigurationBuilder();
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json", false, true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();

            services.AddOptions();
            services.Configure<AppConfig>(Configuration.GetSection(nameof(AppConfig)))
                .AddSingleton<IConfigurationReader, ConfigurationReader>()
                .BuildServiceProvider();

            var serviceProvider = services.BuildServiceProvider();

            ConfigurationReader = serviceProvider.GetService<IConfigurationReader>();

            var config = ConfigurationReader.GetConfiguration();

            ApiKey = config?.ApiKey;
            ApiUrl = config?.ApiUrl;
            Token = config?.Token;
        }
    }
}