using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IReleaseService
    {
        Task<IEnumerable<Release>> GetReleases();

        Task<IEnumerable<Release>> GetReleases(PagingParameters parameters);

        Task<IEnumerable<Release>> Find(Expression<Func<Release, bool>> predicate);



    }
}
