using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Downgrooves.Persistence
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        private IQueryable<Video> _videos;

        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public VideoRepository(DowngroovesDbContext context) : base(context)
        {
            _videos = from video in DowngroovesDbContext.Videos
                      .Include(x => x.Thumbnails)
                      select video;
        }

        public async Task<Video> GetVideo(int id)
        {
            return await _videos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Video>> GetVideos()
        {
            return await _videos.ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetVideos(PagingParameters parameters)
        {
            return await _videos
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
}