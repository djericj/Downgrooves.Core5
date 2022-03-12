using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IVideoService
    {
        IEnumerable<Video> GetVideos();

        IEnumerable<Video> GetVideos(PagingParameters parameters);

        Task<IEnumerable<Video>> GetVideosAsync();


        Task<IEnumerable<Video>> GetVideosAsync(PagingParameters parameters);

        Video Add(Video video);

        IEnumerable<Video> AddRange(IEnumerable<Video> videos);

        IEnumerable<Video> Find(Expression<Func<Video, bool>> predicate);

        Video Update(Video video);

        void Remove(Video video);
    }
}