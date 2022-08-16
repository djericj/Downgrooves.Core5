using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class MixService : IMixService
    {
        private IUnitOfWork _unitOfWork;

        public MixService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Mix> Add(Mix mix)
        {
            await _unitOfWork.Mixes.AddMixAsync(mix);
            await _unitOfWork.CompleteAsync();
            return mix;
        }

        public async Task<MixTrack> AddTrack(MixTrack mixTrack)
        {
            _unitOfWork.MixTracks.Add(mixTrack);
            await _unitOfWork.CompleteAsync();
            return mixTrack;
        }

        public async Task<IEnumerable<MixTrack>> AddTracks(IEnumerable<MixTrack> mixTracks)
        {
            _unitOfWork.MixTracks.AddRange(mixTracks);
            await _unitOfWork.CompleteAsync();
            return mixTracks;
        }

        public async Task<IEnumerable<Mix>> GetMixes()
        {
            return await _unitOfWork.Mixes.GetAllAsync();
        }

        public async Task<IEnumerable<Mix>> GetMixes(PagingParameters parameters)
        {
            return await _unitOfWork.Mixes.GetMixes(parameters);
        }

        public async Task<IEnumerable<Mix>> GetMixesByCategory(string category)
        {
            return await _unitOfWork.Mixes.FindAsync(x => x.Category.ToUpper().Equals(category.ToUpper()));
        }

        public async Task<IEnumerable<Mix>> GetMixesByGenre(string genre)
        {
            return await _unitOfWork.Mixes.FindAsync(x => x.Genre.Name == genre);
        }

        public async Task<Mix> GetMix(int id)
        {
            return await _unitOfWork.Mixes.GetMix(id);
        }

        public async Task Remove(int id)
        {
            var mix = await _unitOfWork.Mixes.GetAsync(id);
            await _unitOfWork.Mixes.Remove(mix);
            await _unitOfWork.CompleteAsync();
            return;
        }

        public async Task RemoveTrack(int id)
        {
            var track = await _unitOfWork.MixTracks.GetAsync(id);
            await _unitOfWork.MixTracks.Remove(track);
            await _unitOfWork.CompleteAsync();
            return;
        }

        public async Task RemoveTracks(IEnumerable<int> ids)
        {
            foreach (var item in ids)
                await RemoveTrack(item);
        }

        public async Task<Mix> Update(Mix mix)
        {
            _unitOfWork.Mixes.UpdateMix(mix);
            await _unitOfWork.CompleteAsync();
            return mix;
        }

        public async Task<MixTrack> UpdateTrack(MixTrack mixTrack)
        {
            _unitOfWork.MixTracks.UpdateState(mixTrack);
            await _unitOfWork.CompleteAsync();
            return mixTrack;
        }

        public async Task<IEnumerable<MixTrack>> UpdateTracks(IEnumerable<MixTrack> mixTracks)
        {
            foreach (var item in mixTracks)
                await UpdateTrack(item);
            return mixTracks;
        }
    }
}