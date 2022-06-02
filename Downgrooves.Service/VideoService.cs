using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Video> AddVideo(Video video)
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

        public async Task<IEnumerable<Video>> AddVideos(IEnumerable<Video> videos)
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

        public async Task<Video> GetVideo(string id)
        {
            return await _unitOfWork.Videos.GetVideo(id);
        }

        public async Task<IEnumerable<Video>> GetVideos()
        {
            return await _unitOfWork.Videos.GetAllAsync();
        }

        public async Task<IEnumerable<Video>> GetVideos(PagingParameters parameters)
        {
            return await _unitOfWork.Videos.GetVideos(parameters);
        }

        public async Task Remove(string id)
        {
            var video = await GetVideo(id);
            await _unitOfWork.Videos.Remove(video);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Video> UpdateVideo(Video video)
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

        public async Task<Thumbnail> AddThumbnail(Thumbnail thumbnail)
        {
            try
            {
                await _unitOfWork.Thumbnails.AddAsync(thumbnail);
                await _unitOfWork.CompleteAsync();
                return thumbnail;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.AddThumbnail {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Thumbnail>> AddThumbnails(IEnumerable<Thumbnail> thumbnails)
        {
            try
            {
                foreach (var thumbnail in thumbnails)
                    await AddThumbnail(thumbnail);
                return thumbnails;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.AddThumbnails {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Thumbnail>> GetThumbnails(Video video)
        {
            try
            {
                var thumbnails = await _unitOfWork.Thumbnails.GetAllAsync();
                return thumbnails?.Where(x => x.Video?.SourceSystemId == video.SourceSystemId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.GetThumbnails {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Thumbnail> GetThumbnail(int id)
        {
            try
            {
                return await _unitOfWork.Thumbnails.GetAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.GetThumbnail {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveThumbnail(int id)
        {
            try
            {
                var thumbnail = await GetThumbnail(id);
                await _unitOfWork.Thumbnails.Remove(thumbnail);
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.RemoveThumbnail {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveThumbnails(IEnumerable<int> ids)
        {
            try
            {
                foreach (var id in ids)
                    await RemoveThumbnail(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.RemoveThumbnails {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Thumbnail> UpdateThumbnail(Thumbnail thumbnail)
        {
            try
            {
                _unitOfWork.Thumbnails.Update(thumbnail);
                await _unitOfWork.CompleteAsync();
                return thumbnail;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.UpdateThumbnail {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Thumbnail>> UpdateThumbnails(IEnumerable<Thumbnail> thumbnails)
        {
            try
            {
                foreach (var thumbnail in thumbnails)
                    await UpdateThumbnail(thumbnail);
                return thumbnails;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.UpdateThumbnails {ex.Message} {ex.StackTrace}");
                throw;
            }
        }
    }
}