using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        IEnumerable<ITunesTrack> GetTracks();

        ITunesTrack Add(ITunesTrack track);

        IEnumerable<ITunesTrack> AddRange(IEnumerable<ITunesTrack> tracks);

        IEnumerable<ITunesTrack> Find(Expression<Func<ITunesTrack, bool>> predicate);

        ITunesTrack Update(ITunesTrack track);

        void Remove(ITunesTrack track);
    }
}