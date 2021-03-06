using Downgrooves.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetGenres();
    }
}