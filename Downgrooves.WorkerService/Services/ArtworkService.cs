using Downgrooves.Domain.ITunes;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ArtworkService : ApiBase, IArtworkService
    {
        private readonly ILogger<ArtworkService> _logger;
        public string ApiUrl { get; }
        public string Token { get; }
        public string ArtworkBasePath { get; }

        public ArtworkService(IOptions<AppConfig> config, ILogger<ArtworkService> logger) : base(config)
        {
            ApiUrl = config.Value.ApiUrl;
            Token = config.Value.Token;
            ArtworkBasePath = config.Value.ArtworkBasePath;
            _logger = logger;
        }

        public async Task GetArtwork(string type)
        {
            try
            {
                var response = await ApiGet($"itunes/{type}");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var collections = JsonConvert.DeserializeObject<ITunesCollection[]>(response.Content);
                    if (collections != null)
                        await GetArtwork(collections, type);
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

        private async Task GetArtwork(IEnumerable<ITunesCollection> collections, string type)
        {
            foreach (var item in collections)
                await GetArtwork(item, type);
        }

        private async Task GetArtwork(ITunesCollection collection, string type)
        {
            var fileName = collection.CollectionId.ToString();
            var imagePath = Path.Combine(ArtworkBasePath, type, $"{fileName}.jpg");
            if (!File.Exists(imagePath))
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        await client.DownloadFileTaskAsync(new Uri(collection.ArtworkUrl100.Replace("100x100", "500x500")), $"{imagePath}");
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