using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IVideoService
    {
        Task<Video> Add(Video video);

        Task<IEnumerable<Video>> AddRange(IEnumerable<Video> videos);

        Task<Video> GetVideo(string id);

        Task<IEnumerable<Video>> GetVideos();

        Task<IEnumerable<Video>> GetVideos(PagingParameters parameters);

        Task Remove(string id);

        Task<Video> Update(Video video);
    }
}