using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        IEnumerable<ITunesTrack> GetTracks();

        Task<IEnumerable<ITunesTrack>> GetTracksAsync();

        ITunesTrack Add(ITunesTrack track);

        IEnumerable<ITunesTrack> AddRange(IEnumerable<ITunesTrack> tracks);

        IEnumerable<ITunesTrack> Find(Expression<Func<ITunesTrack, bool>> predicate);

        ITunesTrack Update(ITunesTrack track);

        void Remove(ITunesTrack track);
    }
}