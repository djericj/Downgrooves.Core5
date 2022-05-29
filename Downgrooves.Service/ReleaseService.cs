using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using Downgrooves.Domain.ITunes;

namespace Downgrooves.Service
{
    public class ReleaseService : IReleaseService
    {
        private readonly ILogger<IReleaseService> _logger;
        private IUnitOfWork _unitOfWork;

        public ReleaseService(IUnitOfWork unitOfWork, ILogger<ReleaseService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Release> Add(Release release)
        {
            try
            {
                _unitOfWork.Releases.Add(release);
                await _unitOfWork.CompleteAsync();
                return release;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Release>> AddRange(IEnumerable<Release> releases)
        {
            try
            {
                _unitOfWork.Releases.AddRange(releases);
                await _unitOfWork.CompleteAsync();
                return releases;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ReleaseTrack> AddTrack(ReleaseTrack releaseTrack)
        {
            try
            {
                _unitOfWork.ReleaseTracks.Add(releaseTrack);
                await _unitOfWork.CompleteAsync();
                return releaseTrack;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ReleaseTrack>> AddTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            try
            {
                _unitOfWork.ReleaseTracks.AddRange(releaseTracks);
                await _unitOfWork.CompleteAsync();
                return releaseTracks;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public List<ITunesExclusion> GetExclusions() => _unitOfWork.Releases.Exclusions;

        public async Task<IEnumerable<Release>> GetReleases(Expression<Func<Release, bool>> predicate)
        {
            return await _unitOfWork.Releases.FindAsync(predicate);
        }

        public async Task<IEnumerable<Release>> GetReleases(string artistName = null)
        {
            return await _unitOfWork.Releases.GetReleases(artistName);
        }

        public async Task<IEnumerable<Release>> GetReleases(PagingParameters parameters, string artistName = null,
            int artistId = 0, bool isOriginal = false, bool isRemix = false)
        {
            return await _unitOfWork.Releases.GetReleases(parameters, artistName, artistId, isOriginal, isRemix);
        }

        public async Task Remove(int id)
        {
            await _unitOfWork.Releases.Remove(id);
        }

        public async Task RemoveTrack(int id)
        {
            await _unitOfWork.ReleaseTracks.Remove(id);
        }

        public async Task RemoveTracks(IEnumerable<int> ids)
        {
            foreach (var item in ids)
                await RemoveTrack(item);
        }

        public async Task<Release> Update(Release collection)
        {
            try
            {
                _unitOfWork.Releases.UpdateState(collection);
                await _unitOfWork.CompleteAsync();
                return collection;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<ReleaseTrack> UpdateTrack(ReleaseTrack releaseTrack)
        {
            try
            {
                _unitOfWork.ReleaseTracks.UpdateState(releaseTrack);
                await _unitOfWork.CompleteAsync();
                return releaseTrack;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.ReleaseService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<ReleaseTrack>> UpdateTracks(IEnumerable<ReleaseTrack> releaseTracks)
        {
            foreach (var item in releaseTracks)
                await UpdateTrack(item);
            return releaseTracks;
        }
    }
}