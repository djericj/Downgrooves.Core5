using Downgrooves.Domain;
using Downgrooves.Persistence.ITunes.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.ITunes
{
    public class CollectionRepository : Repository<ITunesCollection>, ICollectionRepository
    {
        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public CollectionRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections()
        {
            return await DowngroovesDbContext.ITunesCollections.OrderByDescending(x => x.ReleaseDate).ToListAsync();
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(PagingParameters parameters)
        {
            return await DowngroovesDbContext.ITunesCollections
                .OrderByDescending(x => x.ReleaseDate)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
}
