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
        private IUnitOfWork _unitOfWork;
        private readonly ILogger<IVideoService> _logger;

        public VideoService(IUnitOfWork unitOfWork, ILogger<IVideoService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public Video Add(Video video)
        {
            try
            {
                _unitOfWork.Videos.Add(video);
                _unitOfWork.Complete();
                return video;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.Add {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public IEnumerable<Video> AddRange(IEnumerable<Video> videos)
        {
            try
            {
                _unitOfWork.Videos.AddRange(videos);
                _unitOfWork.Complete();
                return videos;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.AddRange {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public IEnumerable<Video> Find(Expression<Func<Video, bool>> predicate)
        {
            return _unitOfWork.Videos.Find(predicate);
        }

        public IEnumerable<Video> GetVideos()
        {
            return _unitOfWork.Videos.GetAll();
        }

        public async Task<IEnumerable<Video>> GetVideosAsync()
        {
            return await Task.Run(() => GetVideos());
        }

        public Video Update(Video video)
        {
            try
            {
                _unitOfWork.Videos.UpdateState(video);
                _unitOfWork.Complete();
                return video;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.Update {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public void Remove(Video video)
        {
            throw new NotImplementedException();
        }
    }
}