using Downgrooves.Model;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Utilities;
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

        public void AddMix(Mix mix)
        {
            _context.Entry(mix.Genre).State = EntityState.Unchanged;
            _context.Set<Mix>().Add(mix);
        }

        public async Task AddMixAsync(Mix mix)
        {
            _context.Entry(mix.Genre).State = EntityState.Unchanged;
            await _context.Set<Mix>().AddAsync(mix);
        }

        public void UpdateMix(Mix mix)
        {
            _context.Entry(mix.Genre).State = EntityState.Unchanged;
            _context.Set<Mix>().Update(mix);
        }

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public async Task<Mix> GetMix(int id)
        {
            return await Task.Run(() => _query
                    .Include(x => x.Tracks)
                    .Include(x => x.Genre)
                    .FirstOrDefault(x => x.Id == id));
        }

        public async Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters)
        {
            _query = _query
                .Include(x => x.Tracks)
                .Include(x => x.Genre)
                .OrderBy(x => x.Title);

            return await GetAllAsync(_query, parameters);
        }
    }
}