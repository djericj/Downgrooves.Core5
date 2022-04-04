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
        public MixRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public async Task<IEnumerable<Mix>> GetMixes()
        {
            return await GetDbSet().ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters)
        {
            return await GetDbSet()
                .OrderBy(x => x.Name)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetMixesByCategory(string category)
        {
            return await GetDbSet().Where(x => x.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetMixesByGenre(string genre)
        {
            return await GetDbSet().Where(x => string.Compare(x.Genre.Name, genre, true) == 0).ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetShowMixes()
        {
            return await GetDbSet().Where(x => x.Show == 1).ToListAsync();
        }

        public async Task<IEnumerable<Mix>> GetShowMixes(PagingParameters parameters)
        {
            return await GetDbSet()
                .OrderBy(x => x.Name)
                .Where(x => x.Show == 1)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        private IQueryable<Mix> GetDbSet()
        {
            return DowngroovesDbContext.Mixes.Include("Genre").Include("Tracks");
        }
    }
}