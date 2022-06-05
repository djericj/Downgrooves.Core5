using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IMixRepository : IRepository<Mix>
    {
        void AddMix(Mix mix);

        Task AddMixAsync(Mix mix);

        Task<Mix> GetMix(int id);

        void UpdateMix(Mix mix);

        Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters);
    }
}