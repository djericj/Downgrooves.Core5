using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Downgrooves.Domain.ITunes;

namespace Downgrooves.Persistence
{
    public class ReleaseRepository : Repository<Release>, IReleaseRepository
    {
        private IQueryable<Release> _query;

        public ReleaseRepository(DowngroovesDbContext context) : base(context)
        {
            _query = from release in DowngroovesDbContext.Releases
                     where (!Exclusions.Select(x => x.CollectionId).Contains(release.CollectionId))
                     where (!Exclusions.Select(x => x.TrackId).Contains(release.TrackId))
                     select release;
        }

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }
        public List<ITunesExclusion> Exclusions => DowngroovesDbContext.ITunesExclusions.ToList();

        public async Task<IEnumerable<Release>> GetReleases(string artistName = null)
        {
            if (artistName != null)
                _query = _query.Where(x => EF.Functions.Like(x.ArtistName, $"%{artistName}%"));

            return await _query
                .OrderByDescending(x => x.ReleaseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Release>> GetReleases(PagingParameters parameters, string artistName = null)
        {
            if (artistName != null)
                _query = _query.Where(x => EF.Functions.Like(x.ArtistName, $"%{artistName}%"));

            return await _query
                .OrderByDescending(x => x.ReleaseDate)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
}