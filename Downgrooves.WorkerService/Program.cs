using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Downgrooves.WorkerService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Diagnostics;

namespace Downgrooves.WorkerService
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate, applyThemeToRedirectedOutput: true)
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Console.WriteLine(msg);
                Debugger.Break();
            });

            try
            {
                CreateHostBuilder(args)
                .UseSystemd()
                .Build()
                .Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "WorkerService terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));
                    services.AddLogging();
                    services.AddSingleton<IApiDataService, ApiDataService>();
                    services.AddSingleton<IArtistService, ArtistService>();
                    services.AddSingleton<IArtworkService, ArtworkService>();
                    services.AddSingleton<IITunesService, ITunesService>();
                    services.AddSingleton<IReleaseService, ReleaseService>();
                    services.AddHostedService<ProcessWorker>();
                });
    }
}