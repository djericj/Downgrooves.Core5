using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        Task<IEnumerable<ITunesTrack>> GetTracks();

        Task<IEnumerable<ITunesTrack>> GetTracks(PagingParameters parameters);

        Task<IEnumerable<ITunesTrack>> LookupCollection(int collectionId);

        Task<ITunesTrack> Add(ITunesTrack track);

        Task<IEnumerable<ITunesTrack>> AddRange(IEnumerable<ITunesTrack> tracks);

        Task<IEnumerable<ITunesTrack>> Find(Expression<Func<ITunesTrack, bool>> predicate);

        Task<ITunesTrack> Update(ITunesTrack track);

        void Remove(ITunesTrack track);
    }
}