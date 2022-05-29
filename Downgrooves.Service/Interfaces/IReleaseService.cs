using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IReleaseService
    {
        Task<Release> Add(Release release);

        Task<IEnumerable<Release>> AddRange(IEnumerable<Release> releases);

        Task<ReleaseTrack> AddTrack(ReleaseTrack releaseTrack);

        Task<IEnumerable<ReleaseTrack>> AddTracks(IEnumerable<ReleaseTrack> releaseTracks);

        List<ITunesExclusion> GetExclusions();

        Task<IEnumerable<Release>> GetReleases(Expression<Func<Release, bool>> predicate);

        Task<IEnumerable<Release>> GetReleases(string artistName = null);

        Task<IEnumerable<Release>> GetReleases(PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false);

        Task Remove(int id);

        Task RemoveTrack(int id);

        Task RemoveTracks(IEnumerable<int> ids);

        Task<Release> Update(Release collection);

        Task<ReleaseTrack> UpdateTrack(ReleaseTrack releaseTrack);

        Task<IEnumerable<ReleaseTrack>> UpdateTracks(IEnumerable<ReleaseTrack> releaseTracks);
    }
}