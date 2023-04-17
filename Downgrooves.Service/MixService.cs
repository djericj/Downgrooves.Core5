using Downgrooves.Domain;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Downgrooves.Data;
using System.Linq.Expressions;
using System;

namespace Downgrooves.Service
{
    public class MixService : ServiceBase, IMixService
    {
        private readonly MixDao _dao;

        public MixService(IConfiguration configuration) : base(configuration)
        {
            var daoFactory = new DaoFactory(_configuration);

            _dao = daoFactory.Mixes as MixDao;
        }

        public IEnumerable<Mix> GetAll(Expression<Func<Mix, bool>> predicate)
        {
            return _dao.GetAll(predicate);
        }

        public IEnumerable<Mix> GetAll()
        {
            return _dao.GetAll();
        }

        public IEnumerable<Mix> GetByCategory(string category)
        {
            return GetAll(x => x.Category.ToUpper().Equals(category.ToUpper()));
        }

        public IEnumerable<Mix> GetByGenre(string genre)
        {
            return GetAll(x => x.Genre.Name == genre);
        }

        public Mix GetMix(int id)
        {
            return _dao.Get(id);
        }
    }
}