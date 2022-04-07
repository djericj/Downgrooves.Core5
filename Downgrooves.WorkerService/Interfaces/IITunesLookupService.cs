using Downgrooves.Domain.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IITunesLookupService
    {
        Task<ITunesLookupResultItem> LookupCollectionById(int collectionId);

        Task<IEnumerable<ITunesLookupResultItem>> LookupCollections(string searchTerm);

        Task<IEnumerable<ITunesLookupResultItem>> LookupTracks(string searchTerm);

        Task<IEnumerable<ITunesLookupResultItem>> LookupTracksCollectionById(int collectionId);
    }
}