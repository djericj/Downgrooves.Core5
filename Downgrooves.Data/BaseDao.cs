using Downgrooves.Domain;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Downgrooves.Data
{
    public abstract class BaseDao
    {
        private readonly AppConfig _config;

        public string BasePath => _config.JsonDataBasePath;

        protected BaseDao(IOptions<AppConfig> config)
        {
            _config = config.Value;
        }

        public static T Deserialize<T>(string filePath)
        {
            return JsonConvert.DeserializeObject<T>(ReadFile(filePath))!;
        }

        protected static string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
