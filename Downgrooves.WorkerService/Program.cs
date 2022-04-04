using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Downgrooves.WorkerService.Services;
using Downgrooves.WorkerService.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Downgrooves.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .UseSystemd()
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));
                    services.AddLogging();
                    services.AddSingleton<IApiClientService, ApiClientService>();
                    services.AddSingleton<IArtworkService, ArtworkService>();
                    services.AddSingleton<IReleaseService, ReleaseService>();
                    services.AddSingleton<IYouTubeService, YouTubeService>();
                    services.AddHostedService<ITunesLoaderWorker>();
                    services.AddHostedService<YouTubeLoaderWorker>();
                });
    }
}