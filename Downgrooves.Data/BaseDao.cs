using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Downgrooves.Data
{
    public abstract class BaseDao
    {
        private readonly IConfiguration _configuration;

        public string BasePath => _configuration["AppConfig:JsonDataBasePath"];

        protected BaseDao(IConfiguration configuration)
        {
            _configuration = configuration;
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
