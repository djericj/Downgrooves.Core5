using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Persistence
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        private IQueryable<Artist> _query;
        private new DowngroovesDbContext _context;

        public ArtistRepository(DowngroovesDbContext context) : base(context)
        {
            _context = context;
            _query = from artist in context.Artists
                     select artist;
        }

        public async Task<IEnumerable<Artist>> GetArtists()
        {
            return await Task.Run(() => _query);
        }

        public async Task<IEnumerable<Artist>> GetArtistsAndReleases()
        {
            return await _context.Artists
                .Include(x => x.Releases).AsNoTracking()
                .ToListAsync();
        }
    }
}