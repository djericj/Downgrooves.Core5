using System.Collections.Generic;
using System.Threading.Tasks;
using Downgrooves.Admin.Services.Interfaces;
using Downgrooves.Domain;

namespace Downgrooves.Admin.ViewModels
{
    public class IndexViewModel : BaseViewModel
    {
        private IApiService<Log> _logService;

        public IndexViewModel(IApiService<Log> logService)
        {
            _logService = logService;
        }

        public async Task<IEnumerable<Log>> GetLogs()
        {
            return await _logService.GetAll(ApiEndpoint.Logs);
        }
    }
}