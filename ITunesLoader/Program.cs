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
using ITunesLoader.Services;

namespace ITunesLoader
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IConfigurationReader ConfigurationReader { get; set; }
        public static string ApiUrl { get; set; }
        public static string Token { get; set; }

        private static IJEnumerable<JToken> results { get; set; }

        private static void Main(string[] args)
        {
            AppConfigurationBuilder();

            TrackService.ApiUrl = ApiUrl;
            TrackService.Token = Token;

            CollectionService.ApiUrl = ApiUrl;  
            CollectionService.Token = Token;

            var json = GetItunesJson();
            JObject o = JObject.Parse(json);
            results = o.SelectTokens("results").Children();

            AddCollections();
            AddTracks();
            
        }

        private static void AddCollections()
        {
            IEnumerable<ITunesCollection> collectionsToAdd = new List<ITunesCollection>();
            var collections = CollectionService.CreateCollections(results);
            var existingCollections = CollectionService.GetExistingCollections();
            if (existingCollections != null && existingCollections.Count() > 0)
                collectionsToAdd = collections.Where(x => existingCollections.All(y => x.CollectionId != y.CollectionId));
            else
                collectionsToAdd = collections;
            var count = CollectionService.AddNewCollections(collectionsToAdd);
            Console.WriteLine($"{count} collections added.");
        }

        private static void AddTracks()
        {
            IEnumerable<ITunesTrack> tracksToAdd = new List<ITunesTrack>();
            
            var tracks = TrackService.CreateTracks(results);
            var existingTracks =  TrackService.GetExistingTracks();
            if (existingTracks != null && existingTracks.Count() > 0)
                tracksToAdd = tracks.Where(x => existingTracks.All(y => x.TrackId != y.TrackId));
            else
                tracksToAdd = tracks;
            var count = TrackService.AddNewTracks(tracksToAdd);
            Console.WriteLine($"{count} tracks added.");
        }

        private static string GetItunesJson()
        {
            string data = null;
            string url = "https://itunes.apple.com/search/?term=Downgrooves&entity=musicArtist,musicTrack,album,mix,song&media=music&limit=200";
            using (var webClient = new WebClient())
                data = webClient.DownloadString(url);
            return data;
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
            Token = config?.Token;
        }
    }
}