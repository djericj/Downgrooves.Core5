using Downgrooves.Domain;
using Downgrooves.Domain.ITunes;
using System.Collections.Generic;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IITunesService
    {
        ITunesCollection AddCollection(ITunesCollection collection);

        IEnumerable<ITunesCollection> AddCollections(IEnumerable<ITunesCollection> collections);

        ITunesCollection GetCollection(Artist artist = null);

        IEnumerable<ITunesCollection> GetCollections(Artist artist = null);

        ITunesCollection UpdateCollection(ITunesCollection collection);

        IEnumerable<ITunesCollection> UpdateCollections(IEnumerable<ITunesCollection> collections);

        ITunesCollection DeleteCollection(ITunesCollection collection);

        IEnumerable<ITunesCollection> DeleteCollections(IEnumerable<ITunesCollection> collections);

        ITunesTrack AddTrack(ITunesTrack Track);

        IEnumerable<ITunesTrack> AddTracks(IEnumerable<ITunesTrack> Tracks);

        ITunesTrack GetTrack(Artist artist = null);

        IEnumerable<ITunesTrack> GetTracks(Artist artist = null);

        ITunesTrack UpdateTrack(ITunesTrack Track);

        IEnumerable<ITunesTrack> UpdateTracks(IEnumerable<ITunesTrack> Tracks);

        ITunesTrack DeleteTrack(ITunesTrack Track);

        IEnumerable<ITunesTrack> DeleteTracks(IEnumerable<ITunesTrack> Tracks);
    }
}