using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;
using System.Net;

namespace Downgrooves.WorkerService.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<ArtworkService> _logger;
        public string ApiUrl { get; }
        public string Token { get; }
        public string ArtworkBasePath { get; }

        public ArtworkService(IOptions<AppConfig> config, ILogger<ArtworkService> logger)
        {
            _appConfig = config.Value;
            ApiUrl = _appConfig.ApiUrl;
            Token = _appConfig.Token;
            ArtworkBasePath = _appConfig.ArtworkBasePath;
            _logger = logger;
        }

        public void GetArtwork(string type)
        {
            try
            {
                var client = new RestClient(ApiUrl);
                client.Authenticator = new JwtAuthenticator(Token);
                var request = new RestRequest("itunes/" + type, Method.GET);
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var response = client.Get(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var collections = JsonConvert.DeserializeObject<ITunesCollection[]>(response.Content);
                    if (collections != null)
                        GetArtwork(collections, type);
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error adding artwork for collections.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

            }
        }

        private void GetArtwork(ITunesCollection[] collections, string type)
        {
            foreach (var item in collections)
                GetArtwork(item, type);
        }

        private void GetArtwork(ITunesCollection collection, string type)
        {
            var fileName = collection.CollectionId.ToString();
            var imagePath = Path.Combine(ArtworkBasePath, type, $"{fileName}.jpg");
            if (!File.Exists(imagePath))
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile(new Uri(collection.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
                        _logger.LogInformation($"Downloaded artwork {imagePath}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);

                    }

                }
            }
        }
    }
}
