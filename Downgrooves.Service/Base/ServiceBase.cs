using System.IO;
using System.Linq;
using System.Reflection;
using Downgrooves.Domain;
using Microsoft.Extensions.Options;

namespace Downgrooves.Service.Base
{
    public abstract class ServiceBase
    {
        protected readonly AppConfig _config;

        protected ServiceBase(IOptions<AppConfig> config)
        {
            _config = config.Value;
        }
        
        protected static string GetEmbeddedResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new(stream);
            return reader.ReadToEnd();
        }
    }
}