using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using Downgrooves.Domain;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ITunesLoader.Interfaces;
using Newtonsoft.Json;
using System.IO;

namespace ITunesLoader
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IConfigurationReader ConfigurationReader { get; set; }
        public static string ApiUrl { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }

        private static int index = 0;

        private static void Main(string[] args)
        {
            AppConfigurationBuilder();

            var json = GetItunesJson();
            JObject o = JObject.Parse(json);
            IJEnumerable<JToken> results = o.SelectTokens("results").Children();
            var tracks = CreateTracks(results);
            var existingTracks = GetExistingTracks();
            var tracksToAdd = tracks.Where(x => existingTracks.All(y => x.TrackId != y.TrackId));
            AddNewTracks(tracksToAdd);
            Console.WriteLine($"{index} tracks added.");
        }

        private static string GetItunesJson()
        {
            string data = null;
            string url = "https://itunes.apple.com/search?term=Downgrooves&limit=200";
            using (var webClient = new WebClient())
            {
                data = webClient.DownloadString(url);
            }
            return data;
        }

        private static void AddNewTracks(IEnumerable<ITunesTrack> tracks)
        {
            foreach (var track in tracks)
                AddNewTrack(track);
        }

        private static void AddNewTrack(ITunesTrack track)
        {
            var client = new RestClient(ApiUrl);
            client.Authenticator = new HttpBasicAuthenticator(UserName, Password);
            var request = new RestRequest("itunes");
            request.AddJsonBody(track);
            var response = client.Post(request);
            var description = $"{track.ArtistName} - {track.TrackName} ({track.TrackId})";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                index++;
                Console.WriteLine($"Added {description}");
            }
            else
                Console.Error.WriteLine($"Error adding {description}");
        }

        private static IEnumerable<ITunesTrack> GetExistingTracks()
        {
            var client = new RestClient(ApiUrl);
            client.Authenticator = new HttpBasicAuthenticator(UserName, Password);
            var request = new RestRequest("itunes/tracks");
            var response = client.Get(request);
            var json = response.Content;
            var tracks = JsonConvert.DeserializeObject<IEnumerable<ITunesTrack>>(json);
            return tracks;
        }

        private static IEnumerable<ITunesTrack> CreateTracks(IJEnumerable<JToken> tokens)
        {
            var tracks = new List<ITunesTrack>();
            foreach (var item in tokens)
            {
                var track = CreateTrack(item);
                tracks.Add(track);
            }
            return tracks;
        }

        private static ITunesTrack CreateTrack(JToken token)
        {
            return new ITunesTrack()
            {
                ArtistId = Convert.ToInt32(token["artistId"]),
                ArtistName = token["artistName"].ToString(),
                ArtistViewUrl = token["artistViewUrl"].ToString(),
                ArtworkUrl100 = token["artworkUrl100"].ToString(),
                ArtworkUrl30 = token["artworkUrl30"].ToString(),
                ArtworkUrl60 = token["artworkUrl60"].ToString(),
                CollectionCensoredName = token["collectionCensoredName"].ToString(),
                CollectionExplicitness = token["collectionExplicitness"].ToString(),
                CollectionId = Convert.ToInt32(token["collectionId"]),
                CollectionName = token["collectionName"].ToString(),
                CollectionPrice = Convert.ToInt32(token["collectionPrice"]),
                CollectionViewUrl = token["collectionViewUrl"].ToString(),
                Country = token["country"].ToString(),
                Currency = token["currency"].ToString(),
                DiscCount = Convert.ToInt32(token["discCount"]),
                DiscNumber = Convert.ToInt32(token["discNumber"]),
                ReleaseDate = Convert.ToDateTime(token["releaseDate"]),
                IsStreamable = token["isStreamable"].ToString(),
                TrackId = Convert.ToInt32(token["trackId"]),
                Kind = token["kind"].ToString(),
                PreviewUrl = token["previewUrl"].ToString(),
                PrimaryGenreName = token["primaryGenreName"].ToString(),
                TrackCensoredName = token["trackCensoredName"].ToString(),
                TrackCount = Convert.ToInt32(token["trackCount"]),
                TrackExplicitness = token["trackExplicitness"].ToString(),
                TrackName = token["trackName"].ToString(),
                TrackNumber = Convert.ToInt32(token["trackNumber"]),
                TrackPrice = Convert.ToInt32(token["trackPrice"]),
                TrackTimeMillis = Convert.ToInt32(token["trackTimeMillis"]),
                TrackViewUrl = token["trackViewUrl"].ToString(),
                WrapperType = token["wrapperType"].ToString(),
            };
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

            ApiUrl = config?.ApiUrl;
            UserName = config?.UserName;
            Password = config?.Password;
        }
    }
}