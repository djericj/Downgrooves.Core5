using Downgrooves.Model;
using Downgrooves.Persistence.Entites;
using Downgrooves.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IReleaseRepository : IRepository<Release>
    {
        void AddRelease(Release release);

        Task AddReleaseAsync(Release release);

        List<ITunesExclusion> Exclusions { get; }

        Task<IEnumerable<Release>> GetReleases(string artistName = null);

        Task<IEnumerable<Release>> GetReleases(PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false);
    }
}