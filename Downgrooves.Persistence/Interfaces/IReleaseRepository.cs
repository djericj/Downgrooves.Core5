using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IReleaseRepository : IRepository<Release>
    {
        void AddRelease(Release release);

        List<ITunesExclusion> Exclusions { get; }

        IEnumerable<Release> GetReleases(string artistName = null);

        IEnumerable<Release> GetReleases(PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false);
    }
}