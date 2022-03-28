using Downgrooves.Persistence.Interfaces;
using Downgrooves.Persistence.ITunes;
using Downgrooves.Persistence.ITunes.Interfaces;
using System.Threading.Tasks;

namespace Downgrooves.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DowngroovesDbContext _context;

        public IMixRepository Mixes { get; private set; }
        public ITrackRepository ITunesTracks { get; private set; }
        public ICollectionRepository ITunesCollections { get; set; }
        public IUserRepository Users { get; private set; }
        public IVideoRepository Videos { get; private set; }

        public UnitOfWork(DowngroovesDbContext context)
        {
            _context = context;
            Mixes = new MixRepository(context);
            ITunesCollections = new CollectionRepository(context);
            ITunesTracks = new TrackRepository(context);
            Users = new UserRepository(context);
            Videos = new VideoRepository(context);
        }

        public void Complete() => _context.SaveChanges();

        public async Task CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

        public async Task DisposeAsync() => await _context.DisposeAsync();
    }
}