using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Downgrooves.Service.Interfaces
{
    public interface IReleaseService
    {
        IEnumerable<Release> GetAll(Expression<Func<Release, bool>> predicate);

        IEnumerable<Release> GetAll(string artistName = null);

        Release Get(int id);
    }
}