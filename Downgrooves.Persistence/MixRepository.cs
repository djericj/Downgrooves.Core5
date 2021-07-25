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
            return GetDbSet().ToList();
        }

        public IEnumerable<Mix> GetShowMixes()
        {
            return GetDbSet().Where(x => x.Show == 1).ToList();
        }

        public IEnumerable<Mix> GetMixesByCategory(string category)
        {
            return GetDbSet().Where(x => x.Category == category);
        }

        public IEnumerable<Mix> GetMixesByGenre(string genre)
        {
            return GetDbSet().Where(x => string.Compare(x.Genre.Name, genre, true) == 0);
        }

        private IQueryable<Mix> GetDbSet()
        {
            return DowngroovesDbContext.Mixes.Include("Genre").Include("Tracks");
        }
    }
}