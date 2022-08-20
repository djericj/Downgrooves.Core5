using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Downgrooves.Persistence
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        private readonly IQueryable<Video> _videos;

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public VideoRepository(DowngroovesDbContext context) : base(context)
        {
            _videos = from video in DowngroovesDbContext.Videos
                      .Include(x => x.Thumbnails)
                      select video;
        }

        public Video GetVideo(int id)
        {
            return _videos.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Video> GetVideos()
        {
            return _videos.ToList();
        }

        public IEnumerable<Video> GetVideos(PagingParameters parameters)
        {
            return GetAll(_videos, parameters);
        }
    }
}