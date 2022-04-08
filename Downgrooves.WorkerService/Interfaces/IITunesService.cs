using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IITunesService
    {
        Task<ITunesCollection> AddNewCollection(ITunesCollection item);

        Task<IEnumerable<ITunesCollection>> AddNewCollections(IEnumerable<ITunesCollection> items);

        Task<ITunesTrack> AddNewTrack(ITunesTrack item);

        Task<IEnumerable<ITunesTrack>> AddNewTracks(IEnumerable<ITunesTrack> items);

        Task<IEnumerable<ITunesExclusion>> GetExclusions();

        Task<IEnumerable<ITunesCollection>> GetCollections(Artist artist = null);

        Task<IEnumerable<ITunesTrack>> GetTracks(Artist artist = null);
    }
}