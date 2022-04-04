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
    public class VideoService : IVideoService
    {
        private readonly ILogger<IVideoService> _logger;
        private IUnitOfWork _unitOfWork;

        public VideoService(IUnitOfWork unitOfWork, ILogger<IVideoService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Video> Add(Video video)
        {
            try
            {
                _unitOfWork.Videos.Add(video);
                await _unitOfWork.CompleteAsync();
                return video;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Video>> AddRange(IEnumerable<Video> videos)
        {
            try
            {
                _unitOfWork.Videos.AddRange(videos);
                await _unitOfWork.CompleteAsync();
                return videos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Video>> Find(Expression<Func<Video, bool>> predicate)
        {
            return await _unitOfWork.Videos.FindAsync(predicate);
        }

        public async Task<IEnumerable<Video>> GetVideos()
        {
            return await _unitOfWork.Videos.GetVideos();
        }

        public async Task<IEnumerable<Video>> GetVideos(PagingParameters parameters)
        {
            return await _unitOfWork.Videos.GetVideos(parameters);
        }

        public void Remove(Video video)
        {
            throw new NotImplementedException();
        }

        public async Task<Video> Update(Video video)
        {
            try
            {
                _unitOfWork.Videos.UpdateState(video);
                await _unitOfWork.CompleteAsync();
                return video;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}