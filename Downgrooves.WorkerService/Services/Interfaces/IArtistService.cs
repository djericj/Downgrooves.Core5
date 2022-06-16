using Downgrooves.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetArtists();
    }
}