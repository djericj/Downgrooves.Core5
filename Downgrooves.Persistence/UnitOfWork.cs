using Downgrooves.Persistence.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Downgrooves.Persistence.Entites;
using Downgrooves.Model;
using Downgrooves.Model.ITunes;

namespace Downgrooves.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DowngroovesDbContext _context;

        public UnitOfWork(DowngroovesDbContext context)
        {
            _context = context;
            ApiData = new Repository<ApiData>(context);
            Artists = new ArtistRepository(context);
            ITunesExclusion = new Repository<ITunesExclusion>(context);
            ITunesCollection = new Repository<ITunesCollection>(context);
            ITunesTrack = new Repository<ITunesTrack>(context);
            Genres = new Repository<Genre>(context);
            Logs = new Repository<Log>(context);
            Mixes = new MixRepository(context);
            MixTracks = new Repository<MixTrack>(context);
            Releases = new ReleaseRepository(context);
            ReleaseTracks = new Repository<ReleaseTrack>(context);
            Thumbnails = new Repository<Thumbnail>(context);
            Users = new UserRepository(context);
            Videos = new VideoRepository(context);
        }

        public IRepository<ApiData> ApiData { get; set; }
        public IArtistRepository Artists { get; private set; }
        public IRepository<ITunesExclusion> ITunesExclusion { get; private set; }
        public IRepository<ITunesCollection> ITunesCollection { get; private set; }
        public IRepository<ITunesTrack> ITunesTrack { get; private set; }
        public IRepository<Genre> Genres { get; private set; }
        public IRepository<Log> Logs { get; set; }
        public IMixRepository Mixes { get; private set; }
        public IRepository<MixTrack> MixTracks { get; private set; }
        public IReleaseRepository Releases { get; private set; }
        public IRepository<ReleaseTrack> ReleaseTracks { get; private set; }
        public IRepository<Thumbnail> Thumbnails { get; set; }
        public IUserRepository Users { get; private set; }
        public IVideoRepository Videos { get; private set; }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                await _context.Database.OpenConnectionAsync();

                return await command.ExecuteNonQueryAsync();
            }
        }

        public void Complete() => _context.SaveChanges();

        public async Task CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

        public async Task DisposeAsync() => await _context.DisposeAsync();
    }
}