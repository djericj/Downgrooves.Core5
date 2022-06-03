using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task<Video> GetVideo(int id)
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

        public async Task Remove(int id)
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

        public async Task<Thumbnail> AddThumbnail(int videoId, Thumbnail thumbnail)
        {
            try
            {
                thumbnail.VideoId = videoId;
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

        public async Task<IEnumerable<Thumbnail>> AddThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails)
        {
            try
            {
                foreach (var thumbnail in thumbnails)
                    await AddThumbnail(videoId, thumbnail);
                return thumbnails;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.AddThumbnails {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<Thumbnail>> GetThumbnails(int videoId)
        {
            try
            {
                return await _unitOfWork.Thumbnails.FindAsync(x => x.VideoId == videoId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.GetThumbnails {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Thumbnail> GetThumbnail(int thumbnailId)
        {
            try
            {
                return await _unitOfWork.Thumbnails.GetAsync(thumbnailId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in Downgrooves.Service.VideoService.GetThumbnail {ex.Message} {ex.StackTrace}");
                throw;
            }
        }

        public async Task RemoveThumbnail(int thumbnailId)
        {
            try
            {
                var thumbnail = await GetThumbnail(thumbnailId);
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

        public async Task<Thumbnail> UpdateThumbnail(int videoId, Thumbnail thumbnail)
        {
            try
            {
                thumbnail.VideoId = videoId;
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

        public async Task<IEnumerable<Thumbnail>> UpdateThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails)
        {
            try
            {
                foreach (var thumbnail in thumbnails)
                    await UpdateThumbnail(videoId, thumbnail);
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