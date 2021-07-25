using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        IEnumerable<Video> GetVideos();
    }
}