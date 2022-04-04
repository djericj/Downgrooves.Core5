using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IReleaseService
    {
        void AddCollections(IEnumerable<Release> collections);

        void AddTracks(IEnumerable<Release> tracks);
    }
}