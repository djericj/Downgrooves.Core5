using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IITunesRepository : IRepository<ITunesTrack>
    {
        IEnumerable<ITunesTrack> GetTracks();
    }
}