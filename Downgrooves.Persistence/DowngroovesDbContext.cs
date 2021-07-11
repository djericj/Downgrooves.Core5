using Downgrooves.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Downgrooves.Persistence
{
    public class DowngroovesDbContext : DbContext
    {
        private string _connectionString;

        public DowngroovesDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }

        public DbSet<Mix> Mixes { get; set; }
        public DbSet<ITunesTrack> ITunesTracks { get; set; }
    }
}