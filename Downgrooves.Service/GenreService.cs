using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Downgrooves.Service
{
    public class GenreService : ServiceBase, IGenreService
    {
        public GenreService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
        }

        public IEnumerable<Genre> GetGenres() => _unitOfWork.Genres.GetAll();
    }
}