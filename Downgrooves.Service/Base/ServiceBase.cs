using Downgrooves.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Downgrooves.Service.Base
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IConfiguration _configuration;

        public ServiceBase(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        protected async Task<int> ExecuteSql(string sql)
        {
            return await _unitOfWork.ExecuteNonQueryAsync(sql);
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