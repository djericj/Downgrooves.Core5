using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IMixService
    {
        Mix Add(Mix mix);

        IEnumerable<Mix> GetMixes();

        IEnumerable<Mix> GetMixes(PagingParameters parameters);

        Task<IEnumerable<Mix>> GetMixesAsync();

        Task<IEnumerable<Mix>> GetMixesAsync(PagingParameters parameters);

        IEnumerable<Mix> GetShowMixes();

        IEnumerable<Mix> GetShowMixes(PagingParameters parameters);

        Task<IEnumerable<Mix>> GetShowMixesAsync();

        Task<IEnumerable<Mix>> GetShowMixesAsync(PagingParameters parameters);

        IEnumerable<Mix> GetMixesByCategory(string category);

        IEnumerable<Mix> GetMixesByGenre(string genre);
    }
}