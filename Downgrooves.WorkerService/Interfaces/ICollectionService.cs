using Downgrooves.Domain.ITunes;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface ICollectionService
    {
        void AddCollections(IEnumerable<JToken> tokens);

        int AddNewCollections(IEnumerable<ITunesCollection> collections);

        IEnumerable<ITunesCollection> GetExistingCollections();

        IEnumerable<ITunesCollection> CreateCollections(IEnumerable<JToken> tokens);
    }
}
