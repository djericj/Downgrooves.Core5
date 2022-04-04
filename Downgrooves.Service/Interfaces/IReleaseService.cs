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

        List<ITunesExclusion> GetExclusions();

        Task<IEnumerable<Release>> GetReleases(Expression<Func<Release, bool>> predicate);

        Task<IEnumerable<Release>> GetReleases(string artistName = null);

        Task<IEnumerable<Release>> GetReleases(PagingParameters parameters, string artistName = null);

        void Remove(int Id);

        void Remove(Release release);

        Task<Release> Update(Release collection);
    }
}