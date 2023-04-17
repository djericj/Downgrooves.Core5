using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Downgrooves.Service.Interfaces
{
    public interface IMixService
    {
        IEnumerable<Mix> GetAll(Expression<Func<Mix, bool>> predicate);

        IEnumerable<Mix> GetAll();

        IEnumerable<Mix> GetByCategory(string category);

        IEnumerable<Mix> GetByGenre(string genre);

        Mix GetMix(int id);
    }
}