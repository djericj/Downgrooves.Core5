using ITunesLoader.Interfaces;
using Microsoft.Extensions.Options;
using System;

namespace ITunesLoader
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly AppConfig _appConfig;

        // I’ve injected <em>secrets</em> into the constructor as setup in Program.cs
        public ConfigurationReader(IOptions<AppConfig> appConfig)
        {
            // We want to know if secrets is null so we throw an exception if it is
            _appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
        }

        public AppConfig GetConfiguration() => _appConfig;
    }
}