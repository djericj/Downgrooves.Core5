using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IMixRepository : IRepository<Mix>
    {
        Task<Mix> GetMix(int id);

        Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters);
    }
}