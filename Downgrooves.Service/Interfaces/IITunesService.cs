using Downgrooves.Domain.ITunes;
using System.Collections.Generic;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        ITunesCollection AddCollection(ITunesCollection item);

        IEnumerable<ITunesCollection> AddCollections(IEnumerable<ITunesCollection> items);

        ITunesTrack AddTrack(ITunesTrack item);

        IEnumerable<ITunesTrack> AddTracks(IEnumerable<ITunesTrack> items);

        IEnumerable<ITunesCollection> GetCollections(string artistName = null);

        ITunesCollection GetCollection(int id);

        IEnumerable<ITunesTrack> GetTracks(string artistName = null);

        ITunesTrack GetTrack(int id);

        IEnumerable<ITunesExclusion> GetExclusions();

        IEnumerable<ITunesLookupResultItem> Lookup(int Id);

        IEnumerable<ITunesLookupLog> GetLookupLog();

        ITunesLookupLog GetLookupLog(int Id);

        ITunesLookupLog AddLookupLog(ITunesLookupLog item);

        void RemoveCollection(int Id);

        void RemoveCollections(IEnumerable<int> ids);

        void RemoveTrack(int Id);

        void RemoveTracks(IEnumerable<int> ids);

        ITunesCollection UpdateCollection(ITunesCollection item);

        ITunesTrack UpdateTrack(ITunesTrack item);

        IEnumerable<ITunesCollection> UpdateCollections(IEnumerable<ITunesCollection> items);

        IEnumerable<ITunesTrack> UpdateTracks(IEnumerable<ITunesTrack> items);
    }
}