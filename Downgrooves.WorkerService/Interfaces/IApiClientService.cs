using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IApiClientService
    {
        void AddNewITunesItem(ITunesLookupResultItem item);

        void AddNewITunesItems(IEnumerable<ITunesLookupResultItem> items);

        void AddNewRelease(Release release);

        int AddNewReleases(IEnumerable<Release> releases);

        IEnumerable<ITunesExclusion> GetExclusions();

        IEnumerable<ITunesLookupResultItem> GetITunesLookupResultItems();

        IEnumerable<Release> GetReleases();

        IEnumerable<Video> GetYouTubeVideosJson();

        ITunesLookupResultItem LookupCollectionById(int collectionId);

        IEnumerable<ITunesLookupResultItem> LookupCollections(string searchTerm);

        IEnumerable<ITunesLookupResultItem> LookupTracks(string searchTerm);

        IEnumerable<ITunesLookupResultItem> LookupTracksCollectionById(int collectionId);
    }
}