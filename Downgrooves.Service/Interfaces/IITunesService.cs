using Downgrooves.Domain.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        Task<ITunesCollection> Add(ITunesCollection item);

        Task<IEnumerable<ITunesCollection>> AddRange(IEnumerable<ITunesCollection> items);

        Task<IEnumerable<ITunesCollection>> GetCollections();

        Task<ITunesTrack> Add(ITunesTrack item);

        Task<IEnumerable<ITunesTrack>> AddRange(IEnumerable<ITunesTrack> items);

        Task<IEnumerable<ITunesTrack>> GetTracks();

        Task<IEnumerable<ITunesLookupResultItem>> Lookup(int Id);
    }
}