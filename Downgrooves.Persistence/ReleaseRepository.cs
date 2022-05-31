﻿using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Downgrooves.Domain.ITunes;
using System.Linq.Expressions;
using System;

namespace Downgrooves.Persistence
{
    public class ReleaseRepository : Repository<Release>, IReleaseRepository
    {
        private IQueryable<Release> _query;

        public ReleaseRepository(DowngroovesDbContext context) : base(context)
        {
            _query = from release in context.Releases
                         //.Include(x => x.Artist)
                     select release;
        }

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }
        public List<ITunesExclusion> Exclusions => DowngroovesDbContext.ITunesExclusions.ToList();

        public void AddRelease(Release release)
        {
            //_context.Entry(release.Artist).State = EntityState.Unchanged;
            _context.Set<Release>().Add(release);
        }

        public async Task AddReleaseAsync(Release release)
        {
            //_context.Entry(release.Artist).State = EntityState.Unchanged;
            await _context.Set<Release>().AddAsync(release);
        }

        public override async Task<IEnumerable<Release>> FindAsync(Expression<Func<Release, bool>> predicate)
        {
            return await DowngroovesDbContext.Releases
                .Include(x => x.Tracks.OrderBy(t => t.TrackNumber))
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Release>> GetReleases(string artistName = null)
        {
            if (artistName != null)
                _query = _query.Where(x => EF.Functions.Like(x.ArtistName, $"%{artistName}%"));

            return await _query
                .Include(x => x.Tracks.OrderBy(t => t.TrackNumber))
                .OrderByDescending(x => x.ReleaseDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Release>> GetReleases(PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false)
        {
            if (artistName != null)
                _query = _query.Where(x => EF.Functions.Like(x.ArtistName, $"%{artistName}%"));

            if (artistId > 0)
                _query = _query.Where(x => x.ArtistId == artistId);

            if (isOriginal)
                _query = _query.Where(x => x.IsOriginal);

            if (isRemix)
                _query = _query.Where(x => x.IsRemix);

            return await _query
                .Include(x => x.Tracks.OrderBy(t => t.TrackNumber))
                .OrderByDescending(x => x.ReleaseDate)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
}