using Downgrooves.Domain.ITunes;
using Downgrooves.Persistence.Interfaces;
using System.Linq;

namespace Downgrooves.Persistence
{
    public class ITunesRepository : Repository<ITunesLookupResultItem>, IITunesRepository
    {
        private IQueryable<ITunesLookupResultItem> _query;

        public ITunesRepository(DowngroovesDbContext context) : base(context)
        {
            _query = from item in context.ITunes
                     select item;
        }

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }
    }
}