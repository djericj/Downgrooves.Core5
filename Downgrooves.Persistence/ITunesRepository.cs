using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Downgrooves.Persistence
{
    public class ITunesRepository : Repository<ITunesTrack>, IITunesRepository
    {
        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public ITunesRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public IEnumerable<ITunesTrack> GetTracks()
        {
            return DowngroovesDbContext.ITunesTracks.ToList();
        }

        public IEnumerable<ITunesTrack> GetTracks(PagingParameters parameters)
        {
            return DowngroovesDbContext.ITunesTracks
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();
        }
    }
}