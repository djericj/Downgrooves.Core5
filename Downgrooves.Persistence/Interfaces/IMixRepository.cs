using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IMixRepository : IRepository<Mix>
    {
        IEnumerable<Mix> GetMixesByCategory(string category);

        IEnumerable<Mix> GetMixesByGenre(Genre genre);
    }
}