using System;
using Downgrooves.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ITunesLoader.Interfaces;
using System.IO;
using ITunesLoader.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace ITunesLoader
{
    internal class Program
    {
        public static IConfiguration Configuration { get; set; }

        private static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    Configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                        .AddJsonFile($"appsettings.json")
                        .AddUserSecrets<Program>()
                        .AddEnvironmentVariables()
                        .Build();
                    services.AddHostedService<ConsoleHostedService>();
                    services.AddSingleton<IITunesService, ITunesService>();
                    services.AddSingleton<ICollectionService, CollectionService>();
                    services.AddSingleton<ITrackService, TrackService>();
                    services.AddSingleton<IConfiguration>(Configuration);
                    services.AddOptions<AppSettings>().Bind(hostContext.Configuration.GetSection("AppConfig"));
                })
                
                .RunConsoleAsync();
        }
    }

    internal sealed class ConsoleHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ICollectionService _collectionService;
        private readonly ITrackService _trackService;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IHostApplicationLifetime appLifetime,
            ICollectionService collectionService,
            ITrackService trackService
            )
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _collectionService = collectionService;
            _trackService = trackService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        // start here
                        var artists = new string[] { "Downgrooves", "Eric Rylos", "Evotone" };

                        foreach (var artist in artists)
                        {
                            _collectionService.AddCollections(artist);
                            _trackService.AddTracks(artist);
                        }
                        _appLifetime.StopApplication();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unhandled exception!");
                    }
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // stop here
            return Task.CompletedTask;
        }
    }
}