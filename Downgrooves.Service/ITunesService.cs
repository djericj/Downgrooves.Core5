using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ITunesService : IITunesService
    {
        private readonly ILogger<IReleaseService> _logger;
        private IConfiguration _configuration;

        public ITunesService(ILogger<ReleaseService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Release>> Lookup(int Id)
        {
            try
            {
                var client = new RestClient(_configuration["AppConfig:ITunesLookupUrl"]);
                var request = new RestRequest();
                request.RequestFormat = DataFormat.Json;

                request.AddParameter("country", "us", ParameterType.UrlSegment);
                request.AddParameter("id", Id);
                request.AddParameter("entity", "musicArtist,musicTrack,album,mix,song");
                request.AddParameter("media", "music");
                var response = await client.ExecuteAsync<ITunesLookupResult>(request);
                if (!string.IsNullOrEmpty(response.Content))
                {
                    var lookupResult = JsonConvert.DeserializeObject<ITunesLookupResult>(response.Content);
                    return lookupResult.Results;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
            }
            return null;
        }
    }
}