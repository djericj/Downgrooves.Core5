using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Downgrooves.Service.Interfaces
{
    public interface IVideoService
    {
        IEnumerable<Video> GetAll(Expression<Func<Video, bool>> predicate);

        Video GetVideo(int id);

        IEnumerable<Video> GetVideos();
    }
}