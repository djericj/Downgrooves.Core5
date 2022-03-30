using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        #region Collections

        Task<IEnumerable<ITunesCollection>> GetCollections(string artistName = null);

        Task<IEnumerable<ITunesCollection>> GetCollections(PagingParameters parameters, string artistName = null);

        Task<ITunesCollection> LookupCollection(int collectionId);

        Task<ITunesCollection> Add(ITunesCollection collection);

        Task<IEnumerable<ITunesCollection>> GetCollection(Expression<Func<ITunesCollection, bool>> predicate);

        Task<ITunesCollection> Update(ITunesCollection collection);

        void Remove(ITunesCollection collection);

        #endregion

        #region Tracks

        Task<IEnumerable<ITunesTrack>> GetTracks(string artistName = null);

        Task<IEnumerable<ITunesTrack>> GetTracks(PagingParameters parameters, string artistName = null);

        Task<IEnumerable<ITunesTrack>> LookupTracks(int collectionId);

        Task<ITunesTrack> Add(ITunesTrack track);

        Task<IEnumerable<ITunesTrack>> AddRange(IEnumerable<ITunesTrack> tracks);

        Task<IEnumerable<ITunesTrack>> GetTrack(Expression<Func<ITunesTrack, bool>> predicate);

        Task<ITunesTrack> Update(ITunesTrack track);

        void Remove(ITunesTrack track);

        #endregion
    }
}