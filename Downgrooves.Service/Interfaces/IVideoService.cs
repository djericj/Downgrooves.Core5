using Downgrooves.Domain;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IVideoService
    {
        Task<Video> AddVideo(Video video);

        Task<IEnumerable<Video>> AddVideos(IEnumerable<Video> videos);

        Task<Video> GetVideo(int id);

        Task<IEnumerable<Video>> GetVideos();

        Task<IEnumerable<Video>> GetVideos(PagingParameters parameters);

        Task Remove(int id);

        Task<Video> UpdateVideo(Video video);

        Task<Thumbnail> AddThumbnail(int videoId, Thumbnail thumbnail);

        Task<IEnumerable<Thumbnail>> AddThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails);

        Task<Thumbnail> GetThumbnail(int thumbnailId);

        Task<IEnumerable<Thumbnail>> GetThumbnails(int videoId);

        Task RemoveThumbnail(int thumbnailId);

        Task RemoveThumbnails(IEnumerable<int> ids);

        Task<Thumbnail> UpdateThumbnail(int videoId, Thumbnail thumbnail);

        Task<IEnumerable<Thumbnail>> UpdateThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails);
    }
}