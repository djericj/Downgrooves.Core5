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
        private readonly IITunesService _iTunesService;
        private readonly ILogger<IReleaseService> _logger;
        private IUnitOfWork _unitOfWork;

        public ReleaseService(IUnitOfWork unitOfWork, ILogger<ReleaseService> logger, IITunesService iTunesService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _iTunesService = iTunesService;
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

        public List<ITunesExclusion> GetExclusions() => _unitOfWork.Releases.Exclusions;

        public async Task<IEnumerable<Release>> GetReleases(Expression<Func<Release, bool>> predicate)
        {
            var releases = new List<Release>();
            var release = await _unitOfWork.Releases.FindAsync(predicate);
            //foreach (var item in release)
            //{
            //    var newRelease = item;
            //    newRelease.Tracks = await _iTunesService.Lookup(item.CollectionId);
            //    releases.Add(newRelease);
            //}
            return release;
        }

        public async Task<IEnumerable<Release>> GetReleases(string artistName = null)
        {
            return await _unitOfWork.Releases.GetReleases(artistName);
        }

        public async Task<IEnumerable<Release>> GetReleases(PagingParameters parameters, string artistName = null)
        {
            return await _unitOfWork.Releases.GetReleases(parameters, artistName);
        }

        public async void Remove(int Id)
        {
            var release = await GetReleases(x => x.Id == Id);
            _unitOfWork.Releases.Remove(release.FirstOrDefault());
        }

        public void Remove(Release release)
        {
            _unitOfWork.Releases.Remove(release);
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
    }
}