using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Downgrooves.Persistence
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        private readonly IQueryable<Artist> _query;
        private new readonly DowngroovesDbContext _context;

        public ArtistRepository(DowngroovesDbContext context) : base(context)
        {
            _context = context;
            _query = from artist in context.Artists
                     select artist;
        }

        public IEnumerable<Artist> GetArtists()
        {
            return _query;
        }

        public IEnumerable<Artist> GetArtistsAndReleases()
        {
            return _context.Artists
                .Include(x => x.Releases).AsNoTracking()
                .ToList();
        }
    }
}