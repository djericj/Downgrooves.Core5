using Downgrooves.Domain;
using Downgrooves.Persistence.ITunes.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.ITunes
{
    public class TrackRepository : Repository<ITunesTrack>, ITrackRepository
    {
        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }
        public List<int> Exclusions => DowngroovesDbContext.ITunesExclusions.Select(x => x.TrackId).ToList();

        public TrackRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks()
        {
            return await DowngroovesDbContext.ITunesTracks
                .Where(x => !Exclusions.Contains(x.TrackId))
                .OrderByDescending(x => x.ReleaseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(PagingParameters parameters)
        {
            return await DowngroovesDbContext.ITunesTracks
                .Where(x => !Exclusions.Contains(x.TrackId))
                .OrderByDescending(x => x.ReleaseDate)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        
    }
}
