using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.ITunes.Interfaces
{
    public interface ICollectionRepository : IRepository<ITunesCollection>
    {
        Task<IEnumerable<ITunesCollection>> GetCollections();

        Task<IEnumerable<ITunesCollection>> GetCollections(PagingParameters parameters);
    }
}
