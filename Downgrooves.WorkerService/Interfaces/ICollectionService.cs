using Downgrooves.Domain;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface ICollectionService
    {
        void AddCollections(string artistName);

        int AddNewCollections(IEnumerable<ITunesCollection> collections);

        IEnumerable<ITunesCollection> GetExistingCollections();

        IEnumerable<ITunesCollection> CreateCollections(IJEnumerable<JToken> tokens);
    }
}
