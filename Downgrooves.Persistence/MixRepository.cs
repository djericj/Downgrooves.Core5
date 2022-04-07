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
                     select mix;
        }

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public async Task<Mix> GetMix(int id)
        {
            return await Task.Run(() => _query
                    .Include(x => x.Tracks)
                    .Include(x => x.Genre)
                    .FirstOrDefault(x => x.MixId == id));
        }

        public async Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters)
        {
            return await _query
                .OrderBy(x => x.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
}