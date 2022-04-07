using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IArtworkService
    {
        Task GetArtwork(IEnumerable<ITunesTrack> tracks);

        Task GetArtwork(IEnumerable<ITunesCollection> tracks);

        Task GetArtwork(IEnumerable<Video> videos);
    }
}