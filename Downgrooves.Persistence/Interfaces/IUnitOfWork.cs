using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ApiData> ApiData { get; }
        IArtistRepository Artists { get; }
        IRepository<ITunesExclusion> ITunesExclusion { get; }
        IRepository<ITunesCollection> ITunesCollection { get; }
        IRepository<ITunesTrack> ITunesTrack { get; }
        IRepository<Genre> Genres { get; }
        IRepository<Log> Logs { get; }
        IMixRepository Mixes { get; }
        IRepository<MixTrack> MixTracks { get; }
        IReleaseRepository Releases { get; }
        IRepository<ReleaseTrack> ReleaseTracks { get; }

        IRepository<Thumbnail> Thumbnails { get; }
        IUserRepository Users { get; }
        IVideoRepository Videos { get; }

        int ExecuteNonQuery(string query);

        void Complete();
    }
}