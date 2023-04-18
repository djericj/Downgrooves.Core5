using Downgrooves.Data.Interfaces;
using Downgrooves.Domain;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Downgrooves.Data
{
    public sealed class VideoDao : BaseDao, IDao<Video>
    {
        private readonly IQueryable<Video> _videos;
        private readonly IQueryable<Thumbnail> _thumbnails;

        public VideoDao(IOptions<AppConfig> config) : base(config)
        {
            _videos = GetData(Path.Combine(BasePath, "video.json"));
            _thumbnails = GetThumbnailData(Path.Combine(BasePath, "thumbnail.json"));
        }

        public IQueryable<Video> GetData(string filePath)
        {
            var videos = Deserialize<IEnumerable<Video>>(filePath);

            return videos.AsQueryable();
        }

        public IQueryable<Thumbnail> GetThumbnailData(string filePath)
        {
            var videos = Deserialize<IEnumerable<Thumbnail>>(filePath);

            return videos.AsQueryable();
        }

        public List<Video> GetAll(Expression<Func<Video, bool>> predicate)
        {
            return _videos.Where(predicate).ToList();
        }

        public List<Thumbnail> GetThumbnails(Expression<Func<Thumbnail, bool>> predicate)
        {
            return _thumbnails.Where(predicate).ToList();
        }

        public IEnumerable<Video> GetAll()
        {
            return _videos;
        }

        public Video? Get(int id)
        {
            var video = GetAll().FirstOrDefault(v => v.Id == id);
            if (video != null)
                video.Thumbnails = GetThumbnails(video.Id);

            return video;
        }

        public Video? Get(string name)
        {
            var video = GetAll().FirstOrDefault(v => v.Title == name);
            if (video != null)
                video.Thumbnails = GetThumbnails(video.Id);

            return video;
        }

        private IList<Thumbnail> GetThumbnails(int videoId)
        {
            return _thumbnails.Where(t => t.VideoId == videoId).ToList();
        }


    }
}

