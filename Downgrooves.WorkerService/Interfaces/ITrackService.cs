using Downgrooves.Domain;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface ITrackService
    {
        void AddTracks(string artistName);
        int AddNewTracks(IEnumerable<ITunesTrack> tracks);
        IEnumerable<ITunesTrack> GetExistingTracks();
        IEnumerable<ITunesTrack> CreateTracks(IJEnumerable<JToken> tokens);
    }
}
