using System;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMixRepository Mixes { get; }
        IITunesRepository ITunesTracks { get; }
        IUserRepository Users { get; }
        IVideoRepository Videos { get; }

        void Complete();

        void CompleteAsync();
    }
}