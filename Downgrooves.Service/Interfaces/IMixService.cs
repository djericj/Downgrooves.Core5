using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IMixService
    {
        Task<Mix> Add(Mix mix);

        Task<Mix> Update(Mix mix);

        Task<IEnumerable<Mix>> GetMixes();

        Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters);

        Task<IEnumerable<Mix>> GetMixesByCategory(string category);

        Task<IEnumerable<Mix>> GetMixesByGenre(string genre);

        Task<Mix> GetMix(int id);
    }
}