using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Downgrooves.Service.Base;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Downgrooves.Service
{
    public class VideoService : ServiceBase, IVideoService
    {
        public VideoService(IConfiguration configuration, IUnitOfWork unitOfWork) : base(configuration, unitOfWork)
        {
        }

        public Video AddVideo(Video video)
        {
            _unitOfWork.Videos.Add(video);
            _unitOfWork.Complete();
            return video;
        }

        public IEnumerable<Video> AddVideos(IEnumerable<Video> videos)
        {
            _unitOfWork.Videos.AddRange(videos);
            _unitOfWork.Complete();
            return videos;
        }

        public Video GetVideo(int id)
        {
            return _unitOfWork.Videos.GetVideo(id);
        }

        public IEnumerable<Video> GetVideos()
        {
            return _unitOfWork.Videos.GetAll();
        }

        public IEnumerable<Video> GetVideos(PagingParameters parameters)
        {
            return _unitOfWork.Videos.GetVideos(parameters);
        }

        public void Remove(int id)
        {
            var video = GetVideo(id);
            _unitOfWork.Videos.Remove(video);
            _unitOfWork.Complete();
        }

        public Video UpdateVideo(Video video)
        {
            _unitOfWork.Videos.UpdateState(video);
            _unitOfWork.Complete();
            return video;
        }

        public Thumbnail AddThumbnail(int videoId, Thumbnail thumbnail)
        {
            thumbnail.VideoId = videoId;
            _unitOfWork.Thumbnails.Add(thumbnail);
            _unitOfWork.Complete();
            return thumbnail;
        }

        public IEnumerable<Thumbnail> AddThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails)
        {
            foreach (var thumbnail in thumbnails)
                AddThumbnail(videoId, thumbnail);
            return thumbnails;
        }

        public IEnumerable<Thumbnail> GetThumbnails(int videoId)
        {
            return _unitOfWork.Thumbnails.Find(x => x.VideoId == videoId);
        }

        public Thumbnail GetThumbnail(int thumbnailId)
        {
            return _unitOfWork.Thumbnails.Get(thumbnailId);
        }

        public void RemoveThumbnail(int thumbnailId)
        {
            var thumbnail = GetThumbnail(thumbnailId);
            _unitOfWork.Thumbnails.Remove(thumbnail);
            _unitOfWork.Complete();
        }

        public void RemoveThumbnails(IEnumerable<int> ids)
        {
            foreach (var id in ids)
                RemoveThumbnail(id);
        }

        public Thumbnail UpdateThumbnail(int videoId, Thumbnail thumbnail)
        {
            thumbnail.VideoId = videoId;
            _unitOfWork.Thumbnails.Update(thumbnail);
            _unitOfWork.Complete();
            return thumbnail;
        }

        public IEnumerable<Thumbnail> UpdateThumbnails(int videoId, IEnumerable<Thumbnail> thumbnails)
        {
            foreach (var thumbnail in thumbnails)
                UpdateThumbnail(videoId, thumbnail);
            return thumbnails;
        }
    }
}