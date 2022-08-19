using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class GenreService : ServiceBase, IGenreService
    {
        public GenreService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
        }

        public async Task<IEnumerable<Genre>> GetGenres() => await _unitOfWork.Genres.GetAllAsync();
    }
}