using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IVideoService
    {
        Task<Video> AddVideo(Video video);

        Task<IEnumerable<Video>> AddVideos(IEnumerable<Video> videos);

        Task<Video> GetVideo(string id);

        Task<IEnumerable<Video>> GetVideos();

        Task<IEnumerable<Video>> GetVideos(PagingParameters parameters);

        Task Remove(string id);

        Task<Video> UpdateVideo(Video video);

        Task<Thumbnail> AddThumbnail(Thumbnail thumbnail);

        Task<IEnumerable<Thumbnail>> AddThumbnails(IEnumerable<Thumbnail> thumbnails);

        Task<Thumbnail> GetThumbnail(int id);

        Task<IEnumerable<Thumbnail>> GetThumbnails(Video video);

        Task RemoveThumbnail(int id);

        Task RemoveThumbnails(IEnumerable<int> ids);

        Task<Thumbnail> UpdateThumbnail(Thumbnail thumbnail);

        Task<IEnumerable<Thumbnail>> UpdateThumbnails(IEnumerable<Thumbnail> thumbnails);
    }
}