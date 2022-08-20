using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        Video GetVideo(int id);

        IEnumerable<Video> GetVideos();

        IEnumerable<Video> GetVideos(PagingParameters parameters);
    }
}