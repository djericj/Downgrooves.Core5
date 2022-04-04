using Downgrooves.Domain;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IApiClientService
    {
        int AddNewReleases(IEnumerable<Release> releases);

        void AddNewRelease(Release release);

        IEnumerable<int> GetExclusions();

        IEnumerable<Release> GetReleases();

        Release LookupCollectionById(int collectionId);

        IEnumerable<Release> LookupTracksCollectionById(int collectionId);

        IEnumerable<Release> LookupCollections(string searchTerm);

        IEnumerable<Release> LookupTracks(string searchTerm);

        IEnumerable<Video> GetYouTubeVideosJson();
    }
}