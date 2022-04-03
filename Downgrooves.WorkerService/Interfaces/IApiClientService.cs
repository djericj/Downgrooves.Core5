using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IApiClientService
    {
        IEnumerable<JToken> LookupCollectionById(int collectionId);
        IEnumerable<JToken> LookupTracksCollectionById(int collectionId);
        IEnumerable<JToken> LookupCollections(string searchTerm);
        IEnumerable<JToken> LookupTracks(string searchTerm);
        IJEnumerable<JToken> GetYouTubeVideosJson();
    }
}
