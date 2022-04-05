using Downgrooves.Domain.ITunes;
using System;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ITunesCollection> ITunesCollection { get; }
        IRepository<ITunesTrack> ITunesTrack { get; }
        IMixRepository Mixes { get; }
        IReleaseRepository Releases { get; }
        IUserRepository Users { get; }
        IVideoRepository Videos { get; }

        void Complete();

        Task CompleteAsync();
    }
}