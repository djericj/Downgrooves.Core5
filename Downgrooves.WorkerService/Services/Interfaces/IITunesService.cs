using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IITunesService
    {
        void Process();

        Task<ITunesCollection> AddNewCollection(ITunesCollection item);

        Task<IEnumerable<ITunesCollection>> AddNewCollections(IEnumerable<ITunesCollection> items);

        Task<ITunesTrack> AddNewTrack(ITunesTrack item);

        Task<IEnumerable<ITunesTrack>> AddNewTracks(IEnumerable<ITunesTrack> items);

        Task<IEnumerable<ITunesExclusion>> GetExclusions();

        Task<IEnumerable<ITunesCollection>> GetCollectionsFromApi(Artist artist = null);

        Task<IEnumerable<ITunesCollection>> GetCollections(Artist artist);

        Task<IEnumerable<ITunesCollection>> GetCollectionsForRemixes(Artist artist);

        Task<IEnumerable<ITunesTrack>> GetTracks(IEnumerable<ITunesCollection> collections);

        Task<IList<Release>> GetTracks(IEnumerable<Release> releases);

        Task<IEnumerable<ITunesTrack>> GetTracks(Artist artist = null);
    }
}