using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using YoutubeLoader.Interfaces;

namespace YoutubeLoader
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IConfigurationReader ConfigurationReader { get; set; }
        public static string ApiKey { get; set; }

        private static void Main(string[] args)
        {
            AppConfigurationBuilder();
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
        }
    }
}