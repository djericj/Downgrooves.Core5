using Downgrooves.Model.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IArtworkService
    {
        Task DownloadArtwork(IEnumerable<ITunesTrack> tracks);

        Task DownloadArtwork(IEnumerable<ITunesCollection> tracks);
    }
}