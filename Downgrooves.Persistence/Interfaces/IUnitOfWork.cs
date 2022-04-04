using System;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMixRepository Mixes { get; }
        IReleaseRepository Releases { get; }
        IUserRepository Users { get; }
        IVideoRepository Videos { get; }

        void Complete();

        Task CompleteAsync();
    }
}