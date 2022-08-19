using Downgrooves.Domain;
using Downgrooves.Admin.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Downgrooves.Admin.ViewModels.Interfaces;

namespace Downgrooves.Admin.ViewModels
{
    public class ReleaseTrackViewModel : BaseViewModel, IViewModel
    {
        private readonly IApiService<ReleaseTrack> _service;
        private readonly IApiService<Release> _releaseService;

        [Required(ErrorMessage = "Artist name is required.")]
        public string ArtistName { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Preview Url is required.")]
        public string PreviewUrl { get; set; }

        [Required(ErrorMessage = "Price is Required.")]
        public double Price { get; set; }

        public int ReleaseId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public int TrackId { get; set; }

        [Required(ErrorMessage = "Track Number is required.")]
        public int TrackNumber { get; set; }

        [Required(ErrorMessage = "Track time is required.")]
        public int TrackTimeInMilliseconds { get; set; }

        public Release Release { get; set; }

        public ReleaseTrackViewModel(IApiService<ReleaseTrack> service, IApiService<Release> releaseService)
        {
            _service = service;
            _releaseService = releaseService;
        }

        public void Add()
        {
            var releaseTrack = CreateReleaseTrack(this);
            MapToViewModel(_service.Add(releaseTrack, ApiEndpoint.ReleaseTrack));
        }

        public void GetRelease(int id)
        {
            Release = _releaseService.Get(id, ApiEndpoint.Release);
        }

        public void GetReleaseTrack(int id)
        {
            MapToViewModel(_service.Get(id, ApiEndpoint.ReleaseTrack));
        }

        public void Update()
        {
            var releaseTrack = CreateReleaseTrack(this);
            MapToViewModel(_service.Update(releaseTrack, ApiEndpoint.ReleaseTrack));
        }

        public void Remove(int id)
        {
            _service.Remove(id, ApiEndpoint.ReleaseTrack);
        }

        private ReleaseTrack CreateReleaseTrack(ReleaseTrackViewModel viewModel)
        {
            return new ReleaseTrack()
            {
                ArtistName = viewModel.ArtistName,
                Id = viewModel.Id,
                PreviewUrl = viewModel.PreviewUrl,
                Price = viewModel.Price,
                ReleaseId = ReleaseId,
                Title = viewModel.Title,
                TrackId = viewModel.TrackId,
                TrackNumber = viewModel.TrackNumber,
                TrackTimeInMilliseconds = viewModel.TrackTimeInMilliseconds,
            };
        }

        private void MapToViewModel(ReleaseTrack releaseTrack)
        {
            ArtistName = releaseTrack.ArtistName;
            Id = releaseTrack.Id;
            PreviewUrl = releaseTrack.PreviewUrl;
            Price = releaseTrack.Price;
            ReleaseId = releaseTrack.ReleaseId;
            Title = releaseTrack.Title;
            TrackId = releaseTrack.TrackId;
            TrackNumber = releaseTrack.TrackNumber;
            TrackTimeInMilliseconds = releaseTrack.TrackTimeInMilliseconds;
            Release = _releaseService.Get(ReleaseId, ApiEndpoint.Release);
        }
    }
}