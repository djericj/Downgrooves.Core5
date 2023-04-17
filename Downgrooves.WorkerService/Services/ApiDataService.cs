using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Downgrooves.WorkerService.Services
{
    public class ApiDataService : ApiService, IApiDataService
    {
        private readonly IOptions<AppConfig> _config;
        private readonly ILogger _logger;
        public ApiDataService(IOptions<AppConfig> config, ILogger<ApiService> logger) : base(config, logger)
        {
            _config = config;
            _logger = logger;
        }

        public JObject Lookup(string url)
        {
            var data = GetString(url);
            return JObject.Parse(data);
        }

        public string LookupSongs(string id)
        {
            return GetString($"{LookupUrl}?id={id}&entity=song");
        }

        public void GetDataFromITunesApi(string url, string artist, ApiData.ApiDataTypes type)
        {
            int limit = 180;
            int offset = 0;
            int index = 0;

            url = url.Replace("{searchTerm}", artist);
            url = url.Replace("{limit}", limit.ToString());

            while (true)
            {
                index++;

                if (offset > 0)
                    url += $"&offset={offset}";

                _logger.LogInformation($"Getting iteration {index}: {url}");

                var data = GetString(url);
                var obj = JObject.Parse(data);
                var resultCount = Convert.ToInt32(obj["resultCount"]);

                if (resultCount == 0)
                    break;

                _logger.LogInformation($"Found {resultCount} items.");

                var typePath = type == ApiData.ApiDataTypes.iTunesCollection ? "Collections" : "Tracks";
                var filePath = Path.Combine(_config.Value.JsonDataBasePath, "iTunes", typePath, "Artists", $"{artist.Replace(" ", "_")}{(index > 1 ? index.ToString() : string.Empty)}.json");
                
                if (File.Exists(filePath))
                    File.Delete(filePath);

                File.WriteAllText(filePath.ToLower(), obj.SelectToken("$.results")!.ToString());

                if (resultCount > offset)
                {
                    offset += limit * index;
                    System.Threading.Thread.Sleep(5000);
                }
                else
                    break;
            }
        }
    }
}