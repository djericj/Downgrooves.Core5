using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Service.Interfaces
{
    public interface ILogService
    {
        IEnumerable<Log> GetLogs();
    }
}