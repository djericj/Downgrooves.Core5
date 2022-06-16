using Downgrooves.Model;
using Downgrooves.Model.ITunes;
using Downgrooves.Persistence.Entites;
using Microsoft.EntityFrameworkCore;

namespace Downgrooves.Persistence
{
    public class DowngroovesDbContext : DbContext
    {
        public DowngroovesDbContext(DbContextOptions<DowngroovesDbContext> options) : base(options)
        {
        }

        public DbSet<ApiData> ApiData { get; set; }
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
            modelBuilder.Entity<ApiData>().ToTable("apiData");
            modelBuilder.Entity<Artist>().ToTable("artist");
            modelBuilder.Entity<Genre>().ToTable("genre");
            modelBuilder.Entity<ITunesCollection>().ToTable("iTunesCollection");
            modelBuilder.Entity<ITunesExclusion>().ToTable("iTunesExclusion");
            modelBuilder.Entity<ITunesTrack>().ToTable("iTunesTrack");
            modelBuilder.Entity<Log>().ToTable("log");
            modelBuilder.Entity<Mix>().ToTable("mix");
            modelBuilder.Entity<MixTrack>().ToTable("mixTrack");
            modelBuilder.Entity<Release>().ToTable("release");
            modelBuilder.Entity<ReleaseTrack>().ToTable("releaseTrack");
            modelBuilder.Entity<Thumbnail>().ToTable("thumbnail");
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Video>().ToTable("video");

            modelBuilder.Entity<Genre>().HasKey("Id");

            modelBuilder.Entity<ApiData>().Property("Id").HasColumnName("apiDataId");
            modelBuilder.Entity<Artist>().Property("Id").HasColumnName("artistId");
            modelBuilder.Entity<Genre>().Property(typeof(int), "Id").HasColumnName("GenreId");
            modelBuilder.Entity<ITunesCollection>().Property("Id").HasColumnName("CollectionId");
            modelBuilder.Entity<ITunesExclusion>().HasNoKey();
            modelBuilder.Entity<ITunesTrack>().Property("Id").HasColumnName("TrackId");
            modelBuilder.Entity<Mix>().Property("Id").HasColumnName("mixId");
            modelBuilder.Entity<MixTrack>().Property("Id").HasColumnName("TrackId");
            modelBuilder.Entity<Release>().Property("Id").HasColumnName("releaseId");
            modelBuilder.Entity<ReleaseTrack>().Property("Id").HasColumnName("releaseTrackId");
            modelBuilder.Entity<ReleaseTrack>().Property("TrackTimeInMilliseconds").HasColumnName("trackTimeInMillis");
            modelBuilder.Entity<Thumbnail>().Property("Id").HasColumnName("thumbnailId");
            modelBuilder.Entity<User>().Property("Id").HasColumnName("userId");
            modelBuilder.Entity<Video>().Property("Id").HasColumnName("videoId");

            modelBuilder.Entity<Mix>().Ignore("BasePath");
            modelBuilder.Entity<Release>().Ignore("BasePath");
            modelBuilder.Entity<Video>().Ignore("ArtworkUrl");
            modelBuilder.Entity<Video>().Ignore("BasePath");
            modelBuilder.Entity<Video>().Ignore("VideoUrl");

            modelBuilder
                .Entity<ApiData>()
                .Property(e => e.IsChanged)
                .HasConversion<int>();

            modelBuilder.Entity<Mix>().HasOne(x => x.Genre);
            modelBuilder.Entity<Mix>().HasMany(x => x.Tracks);
            modelBuilder.Entity<Release>().HasOne(x => x.Artist);
            modelBuilder.Entity<Release>().HasMany(x => x.Tracks);
            modelBuilder.Entity<Video>().HasMany(x => x.Thumbnails);
        }
    }
}