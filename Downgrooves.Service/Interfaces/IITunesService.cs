using Downgrooves.Domain.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        Task<ITunesCollection> AddCollection(ITunesCollection item);

        Task<IEnumerable<ITunesCollection>> AddCollections(IEnumerable<ITunesCollection> items);

        Task<ITunesTrack> AddTrack(ITunesTrack item);

        Task<IEnumerable<ITunesTrack>> AddTracks(IEnumerable<ITunesTrack> items);

        Task<IEnumerable<ITunesCollection>> GetCollections(string artistName = null);

        Task<IEnumerable<ITunesTrack>> GetTracks(string artistName = null);

        Task<IEnumerable<ITunesExclusion>> GetExclusions();

        Task<IEnumerable<ITunesLookupResultItem>> Lookup(int Id);

        Task RemoveCollection(int Id);

        Task RemoveCollections(IEnumerable<int> ids);

        Task RemoveTrack(int Id);

        Task RemoveTracks(IEnumerable<int> ids);

        Task<ITunesCollection> UpdateCollection(ITunesCollection item);

        Task<ITunesTrack> UpdateTrack(ITunesTrack item);

        Task<IEnumerable<ITunesCollection>> UpdateCollections(IEnumerable<ITunesCollection> items);

        Task<IEnumerable<ITunesTrack>> UpdateTracks(IEnumerable<ITunesTrack> items);
    }
}