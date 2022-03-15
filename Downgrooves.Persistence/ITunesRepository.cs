using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Persistence
{
    public class ITunesRepository : Repository<ITunesTrack>, IITunesRepository
    {
        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public ITunesRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks()
        {
            return await DowngroovesDbContext.ITunesTracks.ToListAsync();
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(PagingParameters parameters)
        {
            return await DowngroovesDbContext.ITunesTracks
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
}