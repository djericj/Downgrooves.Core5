using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IMixRepository : IRepository<Mix>
    {
        Task<IEnumerable<Mix>> GetMixes();

        Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters);

        Task<IEnumerable<Mix>> GetMixesByCategory(string category);

        Task<IEnumerable<Mix>> GetMixesByGenre(string genre);

        Task<IEnumerable<Mix>> GetShowMixes();

        Task<IEnumerable<Mix>> GetShowMixes(PagingParameters parameters);
    }
}