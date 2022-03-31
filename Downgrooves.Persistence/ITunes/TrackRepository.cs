using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Persistence.ITunes.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.ITunes
{
    public class TrackRepository : Repository<ITunesTrack>, ITrackRepository
    {
        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }
        public List<int> Exclusions => DowngroovesDbContext.ITunesExclusions.Where(x => x.TrackId > 0).Select(x => x.TrackId).ToList();

        public TrackRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(string artistName = null)
        {
            var query = from collection in DowngroovesDbContext.ITunesTracks
                        where (!Exclusions.Contains(collection.TrackId))
                        select collection;
            if (artistName != null)
                query = query.Where(x => EF.Functions.Like(x.ArtistName, $"%{artistName}%"));

            return await query
                .OrderByDescending(x => x.ReleaseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks(PagingParameters parameters, string artistName = null)
        {
            var query = from collection in DowngroovesDbContext.ITunesTracks
                        where (!Exclusions.Contains(collection.TrackId))
                        select collection;
            if (artistName != null)
                query = query.Where(x => EF.Functions.Like(x.ArtistName, $"%{artistName}%"));

            return await query
                .OrderByDescending(x => x.ReleaseDate)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
}
