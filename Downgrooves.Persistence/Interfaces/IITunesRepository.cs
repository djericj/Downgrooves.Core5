using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IITunesRepository : IRepository<ITunesTrack>
    {
        Task<IEnumerable<ITunesTrack>> GetTracks();
        Task<IEnumerable<ITunesTrack>> GetTracks(PagingParameters parameters);
    }
}