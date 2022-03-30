using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ITunesLoader.Interfaces;
using System.IO;
using ITunesLoader.Services;
using Microsoft.Extensions.Logging;

namespace ITunesLoader
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static string ApiUrl { get; set; }
        public static string Token { get; set; }

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", false, true)
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            //setup our DI
            var services = new ServiceCollection()
                .AddLogging(logger =>
                {
                    logger.AddConsole();
                    logger.SetMinimumLevel(LogLevel.Information);
                })
                .AddOptions()
                .AddSingleton<IConfiguration>(Configuration)
                .AddSingleton<ICollectionService, CollectionService>()
                .AddSingleton<ITrackService, TrackService>()
                .AddSingleton<IITunesService, ITunesService>()
                .BuildServiceProvider();

            var collectionService = services.GetService<ICollectionService>();
            var trackService = services.GetService<ITrackService>();

            var artists = new string[] { "Downgrooves", "Eric Rylos", "Evotone" };

            foreach (var artist in artists)
            {
                collectionService.AddCollections(artist);
                trackService.AddTracks(artist);
            }
        }

    }
}