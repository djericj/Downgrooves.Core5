using Downgrooves.Data;
using Downgrooves.Domain;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Downgrooves.Service
{
    public class ArtistService : ServiceBase, IArtistService
    {
        private readonly ArtistDao _dao;

        public ArtistService(IConfiguration configuration) : base(configuration)
        {
            var daoFactory = new DaoFactory(_configuration);

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