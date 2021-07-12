using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Downgrooves.Service
{
    public class ITunesService : IITunesService
    {
        private IUnitOfWork _unitOfWork;

        public ITunesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ITunesTrack Add(ITunesTrack track)
        {
            _unitOfWork.ITunesTracks.Add(track);
            _unitOfWork.Complete();
            return track;
        }

        public IEnumerable<ITunesTrack> AddRange(IEnumerable<ITunesTrack> tracks)
        {
            _unitOfWork.ITunesTracks.AddRange(tracks);
            _unitOfWork.Complete();
            return tracks;
        }

        public IEnumerable<ITunesTrack> Find(Expression<Func<ITunesTrack, bool>> predicate)
        {
            return _unitOfWork.ITunesTracks.Find(predicate);
        }

        public IEnumerable<ITunesTrack> GetTracks()
        {
            return _unitOfWork.ITunesTracks.GetAll();
        }

        public ITunesTrack Update(ITunesTrack track)
        {
            _unitOfWork.ITunesTracks.UpdateState(track);
            _unitOfWork.Complete();
            return track;
        }

        public void Remove(ITunesTrack track)
        {
            throw new NotImplementedException();
        }
    }
}