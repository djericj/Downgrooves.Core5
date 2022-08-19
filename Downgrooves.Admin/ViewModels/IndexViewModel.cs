using System.Collections.Generic;
using System.Threading.Tasks;
using Downgrooves.Admin.Services.Interfaces;
using Downgrooves.Domain;

namespace Downgrooves.Admin.ViewModels
{
    public class IndexViewModel : BaseViewModel
    {
        private readonly IApiService<Log> _logService;

        public IndexViewModel(IApiService<Log> logService)
        {
            _logService = logService;
        }

        public IEnumerable<Log> GetLogs()
        {
            return _logService.GetAll(ApiEndpoint.Logs);
        }
    }
}