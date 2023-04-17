using System.Collections.Generic;
using Downgrooves.Domain;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IArtworkService
    {
        void DownloadArtwork(IEnumerable<Release> releases);
    }
}