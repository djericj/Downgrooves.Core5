using Downgrooves.Data;
using Downgrooves.Domain;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Downgrooves.Service
{
    public class GenreService : ServiceBase, IGenreService
    {
        private readonly GenreDao _dao;

        public GenreService(IOptions<AppConfig> config) : base(config)
        {
            var daoFactory = new DaoFactory(config);

            _dao = daoFactory.Genres as GenreDao;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _dao.GetAll();
        }
    }
}