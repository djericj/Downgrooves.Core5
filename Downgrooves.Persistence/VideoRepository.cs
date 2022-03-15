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
        public DowngroovesDbContext DowngroovesDbContext { get => _context as DowngroovesDbContext; }

        public VideoRepository(DowngroovesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Video>> GetVideos()
        {
            return await DowngroovesDbContext.Videos.Include("Thumbnails").ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetVideos(PagingParameters parameters)
        {
            return await DowngroovesDbContext.Videos.Include("Thumbnails")
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }
    }
}