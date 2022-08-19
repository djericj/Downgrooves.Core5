using Downgrooves.Domain.ITunes;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IArtworkService
    {
        void DownloadArtwork(IEnumerable<ITunesTrack> tracks);

        void DownloadArtwork(IEnumerable<ITunesCollection> tracks);
    }
}