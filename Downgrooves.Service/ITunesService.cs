using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class ITunesService : IITunesService
    {
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<ITunesService> _logger;

        public ITunesService(IUnitOfWork unitOfWork, ILogger<ITunesService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public ITunesTrack Add(ITunesTrack track)
        {
            try
            {
                _unitOfWork.ITunesTracks.Add(track);
                _unitOfWork.Complete();
                return track;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public IEnumerable<ITunesTrack> AddRange(IEnumerable<ITunesTrack> tracks)
        {
            try
            {
                _unitOfWork.ITunesTracks.AddRange(tracks);
                _unitOfWork.Complete();
                return tracks;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public IEnumerable<ITunesTrack> Find(Expression<Func<ITunesTrack, bool>> predicate)
        {
            return _unitOfWork.ITunesTracks.Find(predicate);
        }

        public IEnumerable<ITunesTrack> GetTracks()
        {
            return _unitOfWork.ITunesTracks.GetAll();
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracksAsync()
        {
            return await Task.Run(() => GetTracks());
        }

        public ITunesTrack Update(ITunesTrack track)
        {
            try
            {
                _unitOfWork.ITunesTracks.UpdateState(track);
                _unitOfWork.Complete();
                return track;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public void Remove(ITunesTrack track)
        {
            throw new NotImplementedException();
        }
    }
}