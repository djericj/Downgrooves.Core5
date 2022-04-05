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
            ITunes = new ITunesRepository(context);
            Mixes = new MixRepository(context);
            Releases = new ReleaseRepository(context);
            Users = new UserRepository(context);
            Videos = new VideoRepository(context);
        }

        public IITunesRepository ITunes { get; set; }
        public IMixRepository Mixes { get; private set; }
        public IReleaseRepository Releases { get; private set; }
        public IUserRepository Users { get; private set; }
        public IVideoRepository Videos { get; private set; }

        public void Complete() => _context.SaveChanges();

        public async Task CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();

        public async Task DisposeAsync() => await _context.DisposeAsync();
    }
}