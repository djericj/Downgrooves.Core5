using Downgrooves.Data;
using Downgrooves.Domain;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Downgrooves.Service
{
    public class GenreService : ServiceBase, IGenreService
    {
        private readonly GenreDao _dao;

        public GenreService(IConfiguration configuration) : base(configuration)
        {
            var daoFactory = new DaoFactory(_configuration);

            _dao = daoFactory.Genres as GenreDao;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _dao.GetAll();
        }
    }
}