using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<Video> GetVideo(int id);

        Task<IEnumerable<Video>> GetVideos();

        Task<IEnumerable<Video>> GetVideos(PagingParameters parameters);
    }
}