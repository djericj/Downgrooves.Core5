using Downgrooves.Framework.Api;
using Downgrooves.WorkerService.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Downgrooves.WorkerService.Services
{
    public class ApiService : ApiBase
    {
        public ApiService(IOptions<AppConfig> config, ILogger<ApiService> logger) : base(logger)
        {
            ApiUrl = config.Value.ApiUrl;
            Token = config.Value.Token;
            ArtworkBasePath = config.Value.ArtworkBasePath;
        }

        public string Token { get; private set; }

        public string ArtworkBasePath { get; }

        public string ApiUrl { get; set; }

        public Uri GetUri(string path)
        {
            var apiUrl = ApiUrl;
            if (apiUrl.EndsWith("/"))
                apiUrl = apiUrl[0..^1];
            return new Uri($"{apiUrl}/{path}");
        }
    }
}