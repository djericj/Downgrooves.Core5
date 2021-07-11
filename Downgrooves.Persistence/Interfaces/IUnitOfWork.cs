using System;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMixRepository Mixes { get; }
        IITunesRepository ITunesTracks { get; }

        void Complete();
    }
}