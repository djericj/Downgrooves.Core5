using Downgrooves.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Downgrooves.Persistence
{
    public class DowngroovesDbContext : DbContext
    {
        public DowngroovesDbContext(DbContextOptions<DowngroovesDbContext> options) : base(options)
        {
            DbPath = $"{AppDomain.CurrentDomain.BaseDirectory}{System.IO.Path.DirectorySeparatorChar}downgrooves.db";
        }

        public DbSet<Mix> Mixes { get; set; }
        public DbSet<ITunesTrack> ITunesTracks { get; set; }
        public DbSet<User> Users { get; set; }

        public string DbPath { get; private set; }

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

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}