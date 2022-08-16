using Downgrooves.Domain;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ArtistService : ApiBase, IArtistService
    {
        private readonly ILogger<ArtistService> _logger;

        public ArtistService(IOptions<AppConfig> config, ILogger<ArtistService> logger) : base(config)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Artist>> GetArtists()
        {
            try
            {
                var response = await ApiGet($"artists");
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