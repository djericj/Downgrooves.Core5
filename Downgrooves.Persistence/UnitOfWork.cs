using Downgrooves.Persistence.Interfaces;

namespace Downgrooves.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DowngroovesDbContext _context;

        public IMixRepository Mixes { get; private set; }

        public UnitOfWork(DowngroovesDbContext context)
        {
            _context = context;
            Mixes = new MixRepository(context);
        }

        public void Complete() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}