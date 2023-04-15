using Downgrooves.Domain;
using Downgrooves.Framework.Api;
using Downgrooves.WorkerService.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Downgrooves.WorkerService.Services
{
    public class ApiService : ApiBase
    {
        private readonly ILogger<ApiService> _logger;

        public ApiService(IOptions<AppConfig> config, ILogger<ApiService> logger) : base(logger)
        {
            _logger = logger;
            ApiUrl = config.Value.ApiUrl;
            Token = config.Value.Token;
            ArtworkBasePath = config.Value.ArtworkBasePath;
            LookupUrl = config.Value.ITunes.LookupUrl;
            LookupInterval = config.Value.ITunes.LookupInterval;
        }

        public string Token { get; private set; }

        public string ArtworkBasePath { get; }

        public string ApiUrl { get; set; }

        public string LookupUrl { get; set; }
        public int LookupInterval { get; set; }

        public T Add<T>(string resource, T items)
        {
            var response = ApiPost<T>(GetUri(resource), Token, items);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error adding items.  {response.Content}");
            return default;
        }

        public T Get<T>(string resource, Artist artist = null)
        {
            if (artist != null)
                resource += $"?artistName={artist.Name}";
            var response = ApiGet(GetUri(resource), Token);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting items.  {response.Content}");
            return default;
        }

        public T Update<T>(string resource, T items)
        {
            var response = ApiPut<T>(GetUri(resource), Token, items);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error updating items.  {response.Content}");
            return default;
        }

        public T Delete<T>(string resource, T items)
        {
            var response = ApiDelete<T>(GetUri(resource), Token, items);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
            }
            else
                _logger.LogError($"Error updating items.  {response.Content}");
            return default;
        }

        public Uri GetUri(string path)
        {
            var apiUrl = ApiUrl;
            if (apiUrl.EndsWith("/"))
                apiUrl = apiUrl[0..^1];
            return new Uri($"{apiUrl}/{path}");
        }
    }
}