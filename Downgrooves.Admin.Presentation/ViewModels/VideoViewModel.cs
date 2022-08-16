using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Presentation.ViewModels
{
    public class VideoViewModel : BaseViewModel, IViewModel
    {
        private IApiService<Video> _service;

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

        public async Task Add()
        {
            var video = CreateVideo(this);
            MapToViewModel(await _service.Add(video, ApiEndpoint.Video));
        }

        public async Task<IEnumerable<Video>> GetVideos()
        {
            return await _service.GetAll(ApiEndpoint.Videos);
        }

        public async Task GetVideo(int id)
        {
            MapToViewModel(await _service.Get(id, ApiEndpoint.Video));
        }

        public async Task Update()
        {
            var video = CreateVideo(this);
            MapToViewModel(await _service.Update(video, ApiEndpoint.Video));
        }

        public async Task Remove(int id)
        {
            await _service.Remove(id, ApiEndpoint.Video);
        }

        private Video CreateVideo(VideoViewModel videoViewModel)
        {
            return new Video()
            {
                PublishedAt = videoViewModel.PublishedAt.Value,
                Description = videoViewModel.Description,
                ETag = videoViewModel.ETag,
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
            ETag = video.ETag;
            SourceSystemId = video.SourceSystemId;
            Thumbnails = video.Thumbnails;
            Title = video.Title;
            DefaultImage = video.High;
            Id = video.Id;
        }
    }
}