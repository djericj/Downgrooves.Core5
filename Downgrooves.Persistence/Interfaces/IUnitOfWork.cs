using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IArtistRepository Artists { get; }
        IRepository<ITunesExclusion> ITunesExclusion { get; }
        IRepository<ITunesCollection> ITunesCollection { get; }
        IRepository<ITunesTrack> ITunesTrack { get; }
        IMixRepository Mixes { get; }
        IReleaseRepository Releases { get; }
        IRepository<ReleaseTrack> ReleaseTracks { get; }
        IUserRepository Users { get; }
        IVideoRepository Videos { get; }

        void Complete();

        Task CompleteAsync();
    }
}