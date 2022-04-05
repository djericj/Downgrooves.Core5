using System;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IITunesRepository ITunes { get; }
        IMixRepository Mixes { get; }
        IReleaseRepository Releases { get; }
        IUserRepository Users { get; }
        IVideoRepository Videos { get; }

        void Complete();

        Task CompleteAsync();
    }
}