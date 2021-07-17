using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Downgrooves.Persistence
{
    public class MixRepository : Repository<Mix>, IMixRepository
    {
        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public MixRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public IEnumerable<Mix> GetMixes()
        {
            return DowngroovesDbContext.Mixes.Include("Genre").Include("Tracks").ToList();
        }

        public IEnumerable<Mix> GetMixesByCategory(string category)
        {
            return DowngroovesDbContext.Mixes.Where(x => x.Category == category);
        }

        public IEnumerable<Mix> GetMixesByGenre(string genre)
        {
            return DowngroovesDbContext.Mixes.Where(x => string.Compare(x.Genre.Name, genre, true) == 0);
        }
    }
}