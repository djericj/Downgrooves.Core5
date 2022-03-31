using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.ITunes.Interfaces
{
    public interface ICollectionRepository : IRepository<ITunesCollection>
    {
        Task<IEnumerable<ITunesCollection>> GetCollections(string artistName = null);

        Task<IEnumerable<ITunesCollection>> GetCollections(PagingParameters parameters, string artistName = null);
    }
}
