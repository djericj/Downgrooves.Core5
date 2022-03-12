using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Downgrooves.Persistence
{
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public VideoRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public IEnumerable<Video> GetVideos()
        {
            return DowngroovesDbContext.Videos.Include("Thumbnails").ToList();
        }

        public IEnumerable<Video> GetVideos(PagingParameters parameters)
        {
            return DowngroovesDbContext.Videos.Include("Thumbnails")
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();
        }
    }
}