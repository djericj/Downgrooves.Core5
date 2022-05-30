using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class MixService : IMixService
    {
        private readonly ILogger<IMixService> _logger;
        private IUnitOfWork _unitOfWork;

        public MixService(IUnitOfWork unitOfWork, ILogger<IMixService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Mix> Add(Mix mix)
        {
            try
            {
                _unitOfWork.Mixes.Add(mix);
                await _unitOfWork.CompleteAsync();
                return mix;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<MixTrack> AddTrack(MixTrack mixTrack)
        {
            try
            {
                _unitOfWork.MixTracks.Add(mixTrack);
                await _unitOfWork.CompleteAsync();
                return mixTrack;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddTrack {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<MixTrack>> AddTracks(IEnumerable<MixTrack> mixTracks)
        {
            try
            {
                _unitOfWork.MixTracks.AddRange(mixTracks);
                await _unitOfWork.CompleteAsync();
                return mixTracks;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.AddTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
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
            try
            {
                var mix = await _unitOfWork.Mixes.GetAsync(id);
                await _unitOfWork.Mixes.Remove(mix);
                await _unitOfWork.CompleteAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Remove {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveTrack(int id)
        {
            try
            {
                var track = await _unitOfWork.MixTracks.GetAsync(id);
                await _unitOfWork.MixTracks.Remove(track);
                await _unitOfWork.CompleteAsync();
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.RemoveTrack {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveTracks(IEnumerable<int> ids)
        {
            try
            {
                foreach (var item in ids)
                    await RemoveTrack(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.RemoveTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Mix> Update(Mix mix)
        {
            try
            {
                _unitOfWork.Mixes.UpdateState(mix);
                await _unitOfWork.CompleteAsync();
                return mix;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<MixTrack> UpdateTrack(MixTrack mixTrack)
        {
            try
            {
                _unitOfWork.MixTracks.UpdateState(mixTrack);
                await _unitOfWork.CompleteAsync();
                return mixTrack;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.UpdateTrack {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<MixTrack>> UpdateTracks(IEnumerable<MixTrack> mixTracks)
        {
            try
            {
                foreach (var item in mixTracks)
                    await UpdateTrack(item);
                return mixTracks;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ITunesService.UpdateTracks {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}