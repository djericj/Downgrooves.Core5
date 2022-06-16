using Downgrooves.Persistence.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface ILogService
    {
        Task<IEnumerable<Log>> GetLogs();
    }
}