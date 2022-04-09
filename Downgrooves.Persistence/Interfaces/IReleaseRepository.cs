using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IReleaseRepository : IRepository<Release>
    {
        List<ITunesExclusion> Exclusions { get; }

        Task<IEnumerable<Release>> GetReleases(string artistName = null);

        Task<IEnumerable<Release>> GetReleases(PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false);
    }
}