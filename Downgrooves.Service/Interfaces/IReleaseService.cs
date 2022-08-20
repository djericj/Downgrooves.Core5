using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Downgrooves.Service.Interfaces
{
    public interface IReleaseService
    {
        Release Add(Release release);

        ReleaseTrack AddTrack(ReleaseTrack releaseTrack);

        IEnumerable<ReleaseTrack> AddTracks(IEnumerable<ReleaseTrack> releaseTracks);

        List<ITunesExclusion> GetExclusions();

        IEnumerable<Release> GetReleases(Expression<Func<Release, bool>> predicate);

        IEnumerable<Release> GetReleases(string artistName = null);

        IEnumerable<Release> GetReleases(PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false);

        ReleaseTrack GetReleaseTrack(int id);

        void Remove(int id);

        void RemoveTrack(int id);

        void RemoveTracks(IEnumerable<int> ids);

        Release Update(Release release);

        ReleaseTrack UpdateTrack(ReleaseTrack releaseTrack);

        IEnumerable<ReleaseTrack> UpdateTracks(IEnumerable<ReleaseTrack> releaseTracks);
    }
}