using Downgrooves.Model;
using Downgrooves.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service.Interfaces
{
    public interface IMixService
    {
        Task<Mix> Add(Mix mix);

        Task<MixTrack> AddTrack(MixTrack mixTrack);

        Task<IEnumerable<MixTrack>> AddTracks(IEnumerable<MixTrack> mixTracks);

        Task<IEnumerable<Mix>> GetMixes();

        Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters);

        Task<IEnumerable<Mix>> GetMixesByCategory(string category);

        Task<IEnumerable<Mix>> GetMixesByGenre(string genre);

        Task<Mix> GetMix(int id);

        Task Remove(int id);

        Task RemoveTrack(int id);

        Task RemoveTracks(IEnumerable<int> ids);

        Task<Mix> Update(Mix mix);

        Task<MixTrack> UpdateTrack(MixTrack mixTrack);

        Task<IEnumerable<MixTrack>> UpdateTracks(IEnumerable<MixTrack> mixTracks);
    }
}