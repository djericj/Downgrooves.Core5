using Downgrooves.Data;
using Downgrooves.Domain;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Options;

namespace Downgrooves.Service
{
    public class HealthService : ServiceBase, IHealthService
    {
        private readonly ArtistDao _artistDao;
        private readonly GenreDao _genreDao;
        private readonly MixDao _mixDao;
        private readonly ReleaseDao _releaseDao;

        public HealthService(IOptions<AppConfig> config) : base(config)
        {
            var daoFactory = new DaoFactory(config);

            _artistDao = daoFactory.Artists as ArtistDao;
            _genreDao = daoFactory.Genres as GenreDao;
            _mixDao = daoFactory.Mixes as MixDao;
            _releaseDao = daoFactory.Releases as ReleaseDao;
        }

        public void GetHealth()
        {
            var artists = _artistDao.GetAll();
            var genres = _genreDao.GetAll();
            var mixes = _mixDao.GetAll();
            var releases = _releaseDao.GetAll();
        }
    }
}
