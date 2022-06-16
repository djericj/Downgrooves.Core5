using Downgrooves.Persistence.Entites;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class LogService : ILogService
    {
        private IUnitOfWork _unitOfWork;

        public LogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Log>> GetLogs() => await _unitOfWork.Logs.GetAllAsync();
    }
}