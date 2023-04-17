using Downgrooves.Data;
using Downgrooves.Domain;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Downgrooves.Service
{
    public class VideoService : ServiceBase, IVideoService
    {
        private readonly VideoDao _dao;

        public VideoService(IConfiguration configuration) : base(configuration)
        {
            var daoFactory = new DaoFactory(_configuration);

            _dao = daoFactory.Videos as VideoDao;
        }
        public IEnumerable<Video> GetAll(Expression<Func<Video, bool>> predicate)
        {
            return _dao.GetAll(predicate);
        }

        public IEnumerable<Thumbnail> GetThumbnails(Expression<Func<Thumbnail, bool>> predicate)
        {
            return _dao.GetThumbnails(predicate);
        }

        public Video GetVideo(int id)
        {
            return _dao.Get(id);
        }

        public IEnumerable<Video> GetVideos()
        {
            return _dao.GetAll();
        }
    }
}