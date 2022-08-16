using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Presentation.ViewModels
{
    public class ReleaseTrackViewModel : BaseViewModel, IViewModel
    {
        private IApiService<ReleaseTrack> _service;
        private IApiService<Release> _releaseService;

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

        public async Task Add()
        {
            var releaseTrack = CreateReleaseTrack(this);
            MapToViewModel(await _service.Add(releaseTrack, ApiEndpoint.ReleaseTrack));
        }

        public async Task GetRelease(int id)
        {
            Release = await _releaseService.Get(id, ApiEndpoint.Release);
        }

        public async Task GetReleaseTrack(int id)
        {
            MapToViewModel(await _service.Get(id, ApiEndpoint.ReleaseTrack));
        }

        public async Task Update()
        {
            var releaseTrack = CreateReleaseTrack(this);
            MapToViewModel(await _service.Update(releaseTrack, ApiEndpoint.ReleaseTrack));
        }

        public async Task Remove(int id)
        {
            await _service.Remove(id, ApiEndpoint.ReleaseTrack);
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

        private async void MapToViewModel(ReleaseTrack releaseTrack)
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
            Release = await _releaseService.Get(ReleaseId, ApiEndpoint.Release);
        }
    }
}