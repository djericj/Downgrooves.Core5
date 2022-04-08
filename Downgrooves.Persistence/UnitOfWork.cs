using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using Downgrooves.Persistence.Interfaces;
using System.Threading.Tasks;

namespace Downgrooves.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DowngroovesDbContext _context;

        public UnitOfWork(DowngroovesDbContext context)
        {
            _context = context;
            Artists = new Repository<Artist>(context);
            ITunesExclusion = new Repository<ITunesExclusion>(context);
            ITunesCollection = new Repository<ITunesCollection>(context);
            ITunesTrack = new Repository<ITunesTrack>(context);
            Mixes = new MixRepository(context);
            Releases = new ReleaseRepository(context);
            ReleaseTracks = new Repository<ReleaseTrack>(context);
            Users = new UserRepository(context);
            Videos = new VideoRepository(context);
        }

        public IRepository<Artist> Artists { get; private set; }
        public IRepository<ITunesExclusion> ITunesExclusion { get; private set; }
        public IRepository<ITunesCollection> ITunesCollection { get; private set; }
        public IRepository<ITunesTrack> ITunesTrack { get; private set; }
        public IMixRepository Mixes { get; private set; }
        public IReleaseRepository Releases { get; private set; }
        public IRepository<ReleaseTrack> ReleaseTracks { get; private set; }
        public IUserRepository Users { get; private set; }
        public IVideoRepository Videos { get; private set; }

        public void Complete() => _context.SaveChanges();

        public async Task CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

        public async Task DisposeAsync() => await _context.DisposeAsync();
    }
}