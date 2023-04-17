using System;
using System.IO;
using Downgrooves.Domain;
using Downgrooves.WorkerService.Config;
using Downgrooves.WorkerService.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Downgrooves.WorkerService.Services
{
    public class ReleaseService : IReleaseService
    {
        private readonly IOptions<AppConfig> _config;

        public ReleaseService(IOptions<AppConfig> config, ILogger<ApiService> logger)
        {
            _config = config;
        }

        public Release GetRelease(string path)
        {
            var id = Path.GetFileNameWithoutExtension(path);
            var json = Path.Combine(_config.Value.JsonDataBasePath, "iTunes", $"{id}.json");
            var data = File.ReadAllText(json);
            var obj = JArray.Parse(data);
            var release = JsonConvert.DeserializeObject<Release>(obj.SelectToken("$[?(@.wrapperType =='collection')]")?.ToString() ?? string.Empty);
            release.Id = Convert.ToInt32(id);
            return release;
        }
    }
}
