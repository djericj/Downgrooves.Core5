using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IMixRepository : IRepository<Mix>
    {
        IEnumerable<Mix> GetMixes();

        IEnumerable<Mix> GetMixesByCategory(string category);

        IEnumerable<Mix> GetMixesByGenre(string genre);
    }
}