using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IReleaseService
    {
        void AddCollections(IEnumerable<Release> collections);
    }
}