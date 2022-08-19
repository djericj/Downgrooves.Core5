using Downgrooves.Domain;
using Downgrooves.Admin.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Downgrooves.Admin.ViewModels.Interfaces;

namespace Downgrooves.Admin.ViewModels
{
    public class VideoViewModel : BaseViewModel, IViewModel
    {
        private readonly IApiService<Video> _service;

        [Required(ErrorMessage = "Publish date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Publish date must be a date.")]
        public DateTime? PublishedAt { get; set; }

        public IList<Thumbnail> Thumbnails { get; set; }
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ETag is required.")]
        public string ETag { get; set; }

        public string Url { get; set; }

        public string SourceSystemId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        public string DefaultImage { get; set; }

        public VideoViewModel(IApiService<Video> service)
        {
            _service = service;
        }

        public void Add()
        {
            var video = CreateVideo(this);
            MapToViewModel(_service.Add(video, ApiEndpoint.Video));
        }

        public IEnumerable<Video> GetVideos()
        {
            return _service.GetAll(ApiEndpoint.Videos);
        }

        public void GetVideo(int id)
        {
            MapToViewModel(_service.Get(id, ApiEndpoint.Video));
        }

        public void Update()
        {
            var video = CreateVideo(this);
            MapToViewModel(_service.Update(video, ApiEndpoint.Video));
        }

        public void Remove(int id)
        {
            _service.Remove(id, ApiEndpoint.Video);
        }

        private static Video CreateVideo(VideoViewModel videoViewModel)
        {
            return new Video()
            {
                PublishedAt = videoViewModel.PublishedAt.Value,
                Description = videoViewModel.Description,
                SourceSystemId = videoViewModel.SourceSystemId,
                Thumbnails = videoViewModel.Thumbnails,
                Title = videoViewModel.Title,
                Id = videoViewModel.Id
            };
        }

        private void MapToViewModel(Video video)
        {
            PublishedAt = video.PublishedAt;
            Description = video.Description;
            SourceSystemId = video.SourceSystemId;
            Thumbnails = video.Thumbnails;
            Title = video.Title;
            DefaultImage = video.ArtworkUrl;
            Id = video.Id;
        }
    }
}