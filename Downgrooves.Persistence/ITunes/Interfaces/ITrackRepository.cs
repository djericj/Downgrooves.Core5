using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Persistence.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.ITunes.Interfaces
{
    public interface ITrackRepository : IRepository<ITunesTrack>
    {
        Task<IEnumerable<ITunesTrack>> GetTracks(string artistName = null);
        Task<IEnumerable<ITunesTrack>> GetTracks(PagingParameters parameters, string artistName = null);
    }
}
