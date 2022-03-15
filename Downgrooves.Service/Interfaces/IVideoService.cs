using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IVideoService
    {
        Task<IEnumerable<Video>> GetVideos();

        Task<IEnumerable<Video>> GetVideos(PagingParameters parameters);

        Task<Video> Add(Video video);

        Task<IEnumerable<Video>> AddRange(IEnumerable<Video> videos);

        Task<IEnumerable<Video>> Find(Expression<Func<Video, bool>> predicate);

        Task<Video> Update(Video video);

        void Remove(Video video);
    }
}