using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Service.Interfaces
{
    public interface IVideoService
    {
        Video AddVideo(Video video);

        IEnumerable<Video> AddVideos(IEnumerable<Video> videos);

        Video GetVideo(int id);

        IEnumerable<Video> GetVideos();

        IEnumerable<Video> GetVideos(PagingParameters parameters);

        void Remove(int id);

        Video UpdateVideo(Video video);

        Thumbnail AddThumbnail(int videoId, Thumbnail thumbnail);

        IEnumerable<Thumbnail> AddThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails);

        Thumbnail GetThumbnail(int thumbnailId);

        IEnumerable<Thumbnail> GetThumbnails(int videoId);

        void RemoveThumbnail(int thumbnailId);

        void RemoveThumbnails(IEnumerable<int> ids);

        Thumbnail UpdateThumbnail(int videoId, Thumbnail thumbnail);

        IEnumerable<Thumbnail> UpdateThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails);
    }
}