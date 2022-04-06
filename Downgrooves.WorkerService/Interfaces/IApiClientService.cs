using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IApiClientService
    {
        Task<ITunesCollection> AddNewCollection(ITunesCollection item);

        Task<IEnumerable<ITunesCollection>> AddNewCollections(IEnumerable<ITunesCollection> items);

        Task<ITunesTrack> AddNewTrack(ITunesTrack item);

        Task<IEnumerable<ITunesTrack>> AddNewTracks(IEnumerable<ITunesTrack> items);

        Task<Release> AddNewRelease(Release release);

        Task<ReleaseTrack> AddNewReleaseTrack(ReleaseTrack releaseTrack);

        Task<int> AddNewReleases(IEnumerable<Release> releases);

        Task<int> AddNewReleaseTracks(IEnumerable<ReleaseTrack> releaseTracks);

        Task<IEnumerable<ITunesExclusion>> GetExclusions();

        Task<IEnumerable<ITunesCollection>> GetCollections();

        Task<IEnumerable<ITunesTrack>> GetTracks();

        Task<IEnumerable<Release>> GetReleases();

        Task<IEnumerable<Video>> GetYouTubeVideosJson();

        Task<ITunesLookupResultItem> LookupCollectionById(int collectionId);

        Task<IEnumerable<ITunesLookupResultItem>> LookupCollections(string searchTerm);

        Task<IEnumerable<ITunesLookupResultItem>> LookupTracks(string searchTerm);

        Task<IEnumerable<ITunesLookupResultItem>> LookupTracksCollectionById(int collectionId);
    }
}