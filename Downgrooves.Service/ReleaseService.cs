using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Downgrooves.Service.Base;
using Microsoft.Extensions.Configuration;
using Downgrooves.Data;
using Microsoft.Extensions.Options;

namespace Downgrooves.Service
{
    public class ReleaseService : ServiceBase, IReleaseService
    {
        private readonly ReleaseDao _dao;

        public ReleaseService(IOptions<AppConfig> config) : base(config)
        {
            var daoFactory = new DaoFactory(config);

            _dao = daoFactory.Releases as ReleaseDao;
        }
        public IEnumerable<Release> GetAll(Expression<Func<Release, bool>> predicate)
        {
            return _dao.GetAll(predicate);
        }

        public IEnumerable<Release> GetAll(string artistName = null)
        {
            return artistName != null ? GetAll(a => a.Artist != null && string.Compare(a.Artist.Name, artistName, StringComparison.OrdinalIgnoreCase) == 0) : GetAll(a => a.Id > 0);
        }

        public Release Get(int id)
        {
            return _dao.Get(id);
        }
    }
}