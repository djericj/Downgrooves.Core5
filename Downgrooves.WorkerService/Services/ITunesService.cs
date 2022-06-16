using Downgrooves.Model;
using Downgrooves.WorkerService.Base;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services
{
    public class ITunesService : ApiBase, IITunesService
    {
        private readonly ILogger<ITunesService> _logger;

        public ITunesService(IOptions<AppConfig> config, ILogger<ITunesService> logger) : base(config)
        {
            _logger = logger;
        }

        #region Downgrooves iTunes API

        public async Task<IEnumerable<T>> Get<T>(string resource, Artist artist = null)
        {
            if (artist != null)
                resource += $"?artistName={artist.Name}";
            var response = await ApiGet(resource);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Content != null && response.Content != "[]")
                {
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(response.Content);
                }
            }
            else
                _logger.LogError($"Error getting existing itunes items.  {response.Content}");
            return null;
        }

        #endregion Downgrooves iTunes API
    }
}