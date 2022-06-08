using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Microsoft.EntityFrameworkCore;

namespace Downgrooves.Persistence
{
    public class DowngroovesDbContext : DbContext
    {
        public DowngroovesDbContext(DbContextOptions<DowngroovesDbContext> options) : base(options)
        {
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<ITunesExclusion> ITunesExclusions { get; set; }
        public DbSet<ITunesCollection> ITunesCollections { get; set; }
        public DbSet<ITunesTrack> ITunesTracks { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Mix> Mixes { get; set; }
        public DbSet<MixTrack> MixTracks { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mix>()
                .HasOne(x => x.Genre)
                .WithMany(y => y.Mixes)
                .HasForeignKey(z => z.GenreId);

            modelBuilder.Entity<Mix>()
                .HasMany(x => x.Tracks)
                .WithOne(y => y.Mix)
                .HasForeignKey(z => z.MixId);

            modelBuilder.Entity<Video>()
                .HasMany(x => x.Thumbnails)
                .WithOne(y => y.Video)
                .HasForeignKey(z => z.VideoId);

            modelBuilder.Entity<ITunesExclusion>()
                .HasNoKey();

            modelBuilder.Entity<Release>()
                .HasMany(x => x.Tracks)
                .WithOne(y => y.Release)
                .IsRequired();

            //modelBuilder.Entity<Release>()
            //    .HasOne(x => x.Artist)
            //    .WithMany(y => y.Releases)
            //    .IsRequired();
        }
    }
}