using Downgrooves.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetArtists();

        Task<IEnumerable<Artist>> GetArtistsAndReleases();
    }
}