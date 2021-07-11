using Downgrooves.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Downgrooves.Service.Interfaces
{
    public interface IITunesService
    {
        IEnumerable<ITunesTrack> GetTracks();

        void Add(ITunesTrack track);

        void AddRange(IEnumerable<ITunesTrack> tracks);

        IEnumerable<ITunesTrack> Find(Expression<Func<ITunesTrack, bool>> predicate);

        void Remove(ITunesTrack track);
    }
}