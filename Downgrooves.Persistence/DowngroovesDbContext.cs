using Downgrooves.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Downgrooves.Persistence
{
    public class DowngroovesDbContext : DbContext
    {
        public DowngroovesDbContext(DbContextOptions<DowngroovesDbContext> options) : base(options)
        {
        }

        public DbSet<Mix> Mixes { get; set; }
        public DbSet<ITunesTrack> ITunesTracks { get; set; }
    }
}