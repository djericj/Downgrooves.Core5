using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IITunesService
    {
        Task<IEnumerable<T>> Get<T>(string resource, Artist artist = null);
    }
}