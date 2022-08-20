using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Service.Interfaces
{
    public interface IGenreService
    {
        IEnumerable<Genre> GetGenres();
    }
}