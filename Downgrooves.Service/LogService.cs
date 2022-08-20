using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Downgrooves.Service
{
    public class LogService : ServiceBase, ILogService
    {
        public LogService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
        }

        public IEnumerable<Log> GetLogs() => _unitOfWork.Logs.GetAll();
    }
}