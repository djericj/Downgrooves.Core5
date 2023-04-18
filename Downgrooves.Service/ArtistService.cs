using Downgrooves.Data;
using Downgrooves.Domain;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Downgrooves.Service
{
    public class ArtistService : ServiceBase, IArtistService
    {
        private readonly ArtistDao _dao;

        public ArtistService(IOptions<AppConfig> config) : base(config)
        {
            var daoFactory = new DaoFactory(config);

            _dao = daoFactory.Artists as ArtistDao;
        }

        public IEnumerable<Artist> GetArtists()
        {
            return _dao.GetAll();
        }

        public Artist GetArtist(int id)
        {
            return _dao.Get(id);
        }

        public Artist GetArtist(string name)
        {
            return _dao.Get(name);
        }
    }
}