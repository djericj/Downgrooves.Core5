using Downgrooves.Persistence.ITunes.Interfaces;
using System;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMixRepository Mixes { get; }
        ICollectionRepository ITunesCollections { get; }
        ITrackRepository ITunesTracks { get; }
        IUserRepository Users { get; }
        IVideoRepository Videos { get; }

        void Complete();

        Task CompleteAsync();
    }
}