using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Interfaces;

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Service
{
    public class VideoService : IVideoService
    {
        private IUnitOfWork _unitOfWork;

        public VideoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Video> AddVideo(Video video)
        {
            _unitOfWork.Videos.Add(video);
            await _unitOfWork.CompleteAsync();
            return video;
        }

        public async Task<IEnumerable<Video>> AddVideos(IEnumerable<Video> videos)
        {
            _unitOfWork.Videos.AddRange(videos);
            await _unitOfWork.CompleteAsync();
            return videos;
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
            _unitOfWork.Videos.UpdateState(video);
            await _unitOfWork.CompleteAsync();
            return video;
        }

        public async Task<Thumbnail> AddThumbnail(int videoId, Thumbnail thumbnail)
        {
            thumbnail.VideoId = videoId;
            await _unitOfWork.Thumbnails.AddAsync(thumbnail);
            await _unitOfWork.CompleteAsync();
            return thumbnail;
        }

        public async Task<IEnumerable<Thumbnail>> AddThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails)
        {
            foreach (var thumbnail in thumbnails)
                await AddThumbnail(videoId, thumbnail);
            return thumbnails;
        }

        public async Task<IEnumerable<Thumbnail>> GetThumbnails(int videoId)
        {
            return await _unitOfWork.Thumbnails.FindAsync(x => x.VideoId == videoId);
        }

        public async Task<Thumbnail> GetThumbnail(int thumbnailId)
        {
            return await _unitOfWork.Thumbnails.GetAsync(thumbnailId);
        }

        public async Task RemoveThumbnail(int thumbnailId)
        {
            var thumbnail = await GetThumbnail(thumbnailId);
            await _unitOfWork.Thumbnails.Remove(thumbnail);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveThumbnails(IEnumerable<int> ids)
        {
            foreach (var id in ids)
                await RemoveThumbnail(id);
        }

        public async Task<Thumbnail> UpdateThumbnail(int videoId, Thumbnail thumbnail)
        {
            thumbnail.VideoId = videoId;
            _unitOfWork.Thumbnails.Update(thumbnail);
            await _unitOfWork.CompleteAsync();
            return thumbnail;
        }

        public async Task<IEnumerable<Thumbnail>> UpdateThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails)
        {
            foreach (var thumbnail in thumbnails)
                await UpdateThumbnail(videoId, thumbnail);
            return thumbnails;
        }
    }
}