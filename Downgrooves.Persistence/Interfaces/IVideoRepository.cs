using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<IEnumerable<Video>> GetVideos();

        Task<IEnumerable<Video>> GetVideos(PagingParameters parameters);
    }
}