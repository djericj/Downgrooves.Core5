using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace Downgrooves.WorkerService.Services
{
    public class ArtistService : ApiService, IArtistService
    {
        private readonly ILogger<ArtistService> _logger;

        public ArtistService(IOptions<AppConfig> config, ILogger<ArtistService> logger) : base(config, logger)
        {
            _logger = logger;
        }

        public IEnumerable<Artist> GetArtists()
        {
            try
            {
                var response = ApiGet(GetUri("artists"), Token);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Artist[]>(response.Content);
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    // do nothing
                }
                else
                    _logger.LogError($"Error getting artists.  Status:  {response.StatusCode}.  Error: {response.ErrorMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
            return null;
        }
    }
}