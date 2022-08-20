using Downgrooves.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;

namespace Downgrooves.Persistence
{
    public class ReleaseRepository : Repository<Release>, IReleaseRepository
    {
        private IQueryable<Release> _query;

        public ReleaseRepository(DowngroovesDbContext context) : base(context)
        {
            _query = from release in context.Releases
                     .Include(x => x.Tracks.OrderBy(t => t.TrackNumber))
                     .OrderByDescending(x => x.ReleaseDate)
                     select release;
        }

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }
        public List<ITunesExclusion> Exclusions => DowngroovesDbContext.ITunesExclusions.ToList();

        public void AddRelease(Release release)
        {
            _context.Entry(release.Artist).State = EntityState.Unchanged;
            _context.Set<Release>().Add(release);
        }

        public override IEnumerable<Release> Find(Expression<Func<Release, bool>> predicate)
        {
            return _query.Where(predicate).ToList();
        }

        public IEnumerable<Release> GetReleases(string artistName = null)
        {
            if (artistName != null)
                _query = _query.Where(x => EF.Functions.Like(x.ArtistName, $"%{artistName}%"));

            return _query.ToList();
        }

        public IEnumerable<Release> GetReleases(PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false)
        {
            if (artistName != null)
                _query = _query.Where(x => EF.Functions.Like(x.ArtistName, $"%{artistName}%"));

            if (artistId > 0)
                _query = _query.Where(x => x.Id == artistId);

            if (isOriginal)
                _query = _query.Where(x => x.IsOriginal);

            if (isRemix)
                _query = _query.Where(x => x.IsRemix);

            return GetAll(_query, parameters);
        }
    }
}