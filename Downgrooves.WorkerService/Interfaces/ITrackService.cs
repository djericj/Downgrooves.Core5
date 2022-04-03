using Downgrooves.Domain.ITunes;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface ITrackService
    {
        void AddTracks(IEnumerable<JToken> tokens);
        int AddNewTracks(IEnumerable<ITunesTrack> tracks);
        IEnumerable<ITunesTrack> GetExistingTracks();
        IEnumerable<ITunesTrack> CreateTracks(IEnumerable<JToken> tokens);
    }
}
