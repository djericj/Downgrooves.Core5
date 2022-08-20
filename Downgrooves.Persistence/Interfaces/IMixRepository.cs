using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IMixRepository : IRepository<Mix>
    {
        void AddMix(Mix mix);

        Mix GetMix(int id);

        void UpdateMix(Mix mix);

        IEnumerable<Mix> GetMixes(PagingParameters parameters);
    }
}