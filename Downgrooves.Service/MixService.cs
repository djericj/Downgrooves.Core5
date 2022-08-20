using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Downgrooves.Service
{
    public class MixService : ServiceBase, IMixService
    {
        public MixService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
        }

        public Mix Add(Mix mix)
        {
            _unitOfWork.Mixes.AddMix(mix);
            _unitOfWork.Complete();
            return mix;
        }

        public MixTrack AddTrack(MixTrack mixTrack)
        {
            _unitOfWork.MixTracks.Add(mixTrack);
            _unitOfWork.Complete();
            return mixTrack;
        }

        public IEnumerable<MixTrack> AddTracks(IEnumerable<MixTrack> mixTracks)
        {
            _unitOfWork.MixTracks.AddRange(mixTracks);
            _unitOfWork.Complete();
            return mixTracks;
        }

        public IEnumerable<Mix> GetMixes()
        {
            return _unitOfWork.Mixes.GetAll();
        }

        public IEnumerable<Mix> GetMixes(PagingParameters parameters)
        {
            return _unitOfWork.Mixes.GetMixes(parameters);
        }

        public IEnumerable<Mix> GetMixesByCategory(string category)
        {
            return _unitOfWork.Mixes.Find(x => x.Category.ToUpper().Equals(category.ToUpper()));
        }

        public IEnumerable<Mix> GetMixesByGenre(string genre)
        {
            return _unitOfWork.Mixes.Find(x => x.Genre.Name == genre);
        }

        public Mix GetMix(int id)
        {
            return _unitOfWork.Mixes.GetMix(id);
        }

        public void Remove(int id)
        {
            var mix = _unitOfWork.Mixes.Get(id);
            _unitOfWork.Mixes.Remove(mix);
            _unitOfWork.Complete();
        }

        public void RemoveTrack(int id)
        {
            var track = _unitOfWork.MixTracks.Get(id);
            _unitOfWork.MixTracks.Remove(track);
            _unitOfWork.Complete();
        }

        public void RemoveTracks(IEnumerable<int> ids)
        {
            foreach (var item in ids)
                RemoveTrack(item);
        }

        public Mix Update(Mix mix)
        {
            _unitOfWork.Mixes.UpdateMix(mix);
            _unitOfWork.Complete();
            return mix;
        }

        public MixTrack UpdateTrack(MixTrack mixTrack)
        {
            _unitOfWork.MixTracks.UpdateState(mixTrack);
            _unitOfWork.Complete();
            return mixTrack;
        }

        public IEnumerable<MixTrack> UpdateTracks(IEnumerable<MixTrack> mixTracks)
        {
            foreach (var item in mixTracks)
                UpdateTrack(item);
            return mixTracks;
        }
    }
}