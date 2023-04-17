using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Downgrooves.Service.Base;
using Microsoft.Extensions.Configuration;
using Downgrooves.Data;

namespace Downgrooves.Service
{
    public class ReleaseService : ServiceBase, IReleaseService
    {
        private readonly ReleaseDao _dao;

        public ReleaseService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
            var daoFactory = new DaoFactory(_configuration);

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