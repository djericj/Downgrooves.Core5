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
        public List<int> Exclusions => DowngroovesDbContext.ITunesExclusions.Where(x => x.CollectionId > 0).Select(x => x.CollectionId).ToList();

        public CollectionRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections()
        {
            return await DowngroovesDbContext.ITunesCollections
                .Where(x => !Exclusions.Contains(x.CollectionId))
                .OrderByDescending(x => x.ReleaseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ITunesCollection>> GetCollections(PagingParameters parameters)
        {
            return await DowngroovesDbContext.ITunesCollections
                .Where(x => !Exclusions.Contains(x.CollectionId))
                .OrderByDescending(x => x.ReleaseDate)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        
    }
}
