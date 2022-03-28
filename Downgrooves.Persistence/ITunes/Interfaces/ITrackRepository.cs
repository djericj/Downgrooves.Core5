using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.ITunes.Interfaces
{
    public interface ITrackRepository : IRepository<ITunesTrack>
    {
        Task<IEnumerable<ITunesTrack>> GetTracks();
        Task<IEnumerable<ITunesTrack>> GetTracks(PagingParameters parameters);
    }
}
