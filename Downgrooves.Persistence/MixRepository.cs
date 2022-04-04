using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Persistence
{
    public class MixRepository : Repository<Mix>, IMixRepository
    {
        private IQueryable<Mix> _query;

        public MixRepository(DowngroovesDbContext context) : base(context)
        {
            _query = from mix in context.Mixes
                     .Include(x => x.Tracks)
                     .Include(x => x.Genre)
                     select mix;
        }

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public async Task<IEnumerable<Mix>> GetMixes()
        {
            return await _query.ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters)
        {
            return await _query
                .OrderBy(x => x.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetMixesByCategory(string category)
        {
            return await _query.Where(x => x.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetMixesByGenre(string genre)
        {
            return await _query.Where(x => x.Genre.Name == genre).ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetShowMixes()
        {
            return await _query.Where(x => x.Show == 1).ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetShowMixes(PagingParameters parameters)
        {
            return await _query
                .OrderBy(x => x.Name)
                .Where(x => x.Show == 1)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
}