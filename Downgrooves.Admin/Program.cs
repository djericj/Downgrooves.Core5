using Blazored.Toast;
using Downgrooves.Domain;
using Downgrooves.Admin.ViewModels;
using Downgrooves.Admin.Services;
using Downgrooves.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Downgrooves.Domain.ITunes;

namespace Downgrooves.Admin
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            builder.Logging.AddSerilog();

            builder.Configuration.AddUserSecrets<Program>(true);

            var appConfig = builder.Configuration.GetSection("AppConfig").Get<AppConfig>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(appConfig.ApiUrl) });
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });
            builder.Services.AddTransient<ITunesCollectionViewModel>();
            builder.Services.AddTransient<ITunesTrackViewModel>();
            builder.Services.AddTransient<IndexViewModel>();
            builder.Services.AddTransient<MixViewModel>();
            builder.Services.AddTransient<MixTrackViewModel>();
            builder.Services.AddTransient<ReleaseViewModel>();
            builder.Services.AddTransient<ReleaseTrackViewModel>();
            builder.Services.AddTransient<VideoViewModel>();

            builder.Services.AddScoped<IApiService<ITunesCollection>, ApiService<ITunesCollection>>();
            builder.Services.AddScoped<IApiService<ITunesTrack>, ApiService<ITunesTrack>>();
            builder.Services.AddScoped<IApiService<Genre>, ApiService<Genre>>();
            builder.Services.AddScoped<IApiService<Domain.Log>, ApiService<Domain.Log>>();
            builder.Services.AddScoped<IMixService, MixService>();
            builder.Services.AddScoped<IApiService<MixTrack>, ApiService<MixTrack>>();
            builder.Services.AddScoped<IApiService<Release>, ApiService<Release>>();
            builder.Services.AddScoped<IApiService<ReleaseTrack>, ApiService<ReleaseTrack>>();
            builder.Services.AddScoped<IApiService<Video>, ApiService<Video>>();

            builder.Services.AddBlazoredToast();

            builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));

            await builder.Build().RunAsync();
        }
    }
}