using Downgrooves.Domain;
using Microsoft.EntityFrameworkCore;

namespace Downgrooves.Persistence
{
    public class DowngroovesDbContext : DbContext
    {
        public DowngroovesDbContext(DbContextOptions<DowngroovesDbContext> options) : base(options)
        {
        }

        public DbSet<Mix> Mixes { get; set; }
        public DbSet<ITunesTrack> ITunesTracks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mix>()
                .HasOne(x => x.Genre)
                .WithMany(y => y.Mixes)
                .HasForeignKey(z => z.GenreId);

            modelBuilder.Entity<Mix>()
                .HasMany(x => x.Tracks)
                .WithOne(y => y.Mix);
        }
    }
}