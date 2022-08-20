using Downgrooves.Domain;
using System.Collections.Generic;

namespace Downgrooves.Service.Interfaces
{
    public interface IMixService
    {
        Mix Add(Mix mix);

        MixTrack AddTrack(MixTrack mixTrack);

        IEnumerable<MixTrack> AddTracks(IEnumerable<MixTrack> mixTracks);

        IEnumerable<Mix> GetMixes();

        IEnumerable<Mix> GetMixes(PagingParameters parameters);

        IEnumerable<Mix> GetMixesByCategory(string category);

        IEnumerable<Mix> GetMixesByGenre(string genre);

        Mix GetMix(int id);

        void Remove(int id);

        void RemoveTrack(int id);

        void RemoveTracks(IEnumerable<int> ids);

        Mix Update(Mix mix);

        MixTrack UpdateTrack(MixTrack mixTrack);

        IEnumerable<MixTrack> UpdateTracks(IEnumerable<MixTrack> mixTracks);
    }
}