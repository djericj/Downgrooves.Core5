using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Presentation.ViewModels
{
    public class ReleaseViewModel : BaseViewModel, IViewModel
    {
        private IApiService<Release> _releaseService;
        private IApiService<ReleaseTrack> _releaseTrackService;

        public int CollectionId { get; set; }

        public int ArtistId { get; set; }

        //[Required(ErrorMessage = null)]
        public string ArtistName { get; set; }

        //[Required(ErrorMessage = null)]
        public string ArtistViewUrl { get; set; }

        //[Required(ErrorMessage = null)]
        public string ArtworkUrl { get; set; }

        //[Required(ErrorMessage = null)]
        public string BuyUrl { get; set; }

        //[Required(ErrorMessage = null)]
        public string Copyright { get; set; }

        //[Required(ErrorMessage = null)]
        public string Country { get; set; }

        public int DiscCount { get; set; }
        public int DiscNumber { get; set; }

        //[Required(ErrorMessage = null)]
        public string Genre { get; set; }

        public int Id { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsRemix { get; set; }

        //[Required(ErrorMessage = null)]
        public string PreviewUrl { get; set; }

        //[Required(ErrorMessage = null)]
        public double Price { get; set; }

        //[Required(ErrorMessage = null)]
        public DateTime ReleaseDate { get; set; }

        //[Required(ErrorMessage = null)]
        public string Title { get; set; }

        public IEnumerable<ReleaseTrack> Tracks { get; set; }

        //[Required(ErrorMessage = null)]
        public int VendorId { get; set; }

        public ReleaseViewModel(IApiService<Release> releaseService, IApiService<ReleaseTrack> releaseTrackService)
        {
            _releaseService = releaseService;
            _releaseTrackService = releaseTrackService;
        }

        public async Task Add()
        {
            var release = CreateRelease(this);
            MapToViewModel(await _releaseService.Add(release, ApiEndpoint.Release));
        }

        public async Task<IEnumerable<Release>> GetReleases()
        {
            return await _releaseService.GetAll(ApiEndpoint.Releases);
        }

        public async Task GetRelease(int id)
        {
            MapToViewModel(await _releaseService.Get(id, ApiEndpoint.Release));
        }

        public async Task Update()
        {
            var release = CreateRelease(this);
            MapToViewModel(await _releaseService.Update(release, ApiEndpoint.Release));
        }

        public async Task Remove(int id)
        {
            await _releaseService.Remove(id, ApiEndpoint.Release);
        }

        public async Task RemoveTrack(int id)
        {
            await _releaseTrackService.Remove(id, ApiEndpoint.ReleaseTrack);
        }

        private Release CreateRelease(ReleaseViewModel viewModel)
        {
            return new Release()
            {
                ArtistId = viewModel.ArtistId,
                ArtistName = viewModel.ArtistName,
                ArtistViewUrl = viewModel.ArtistViewUrl,
                ArtworkUrl = viewModel.ArtworkUrl,
                BuyUrl = viewModel.BuyUrl,
                CollectionId = viewModel.CollectionId,
                Copyright = viewModel.Copyright,
                Country = viewModel.Country,
                DiscCount = viewModel.DiscCount,
                DiscNumber = viewModel.DiscNumber,
                Id = viewModel.Id,
                IsOriginal = viewModel.IsOriginal,
                IsRemix = viewModel.IsRemix,
                Genre = viewModel.Genre,
                PreviewUrl = viewModel.PreviewUrl,
                Price = viewModel.Price,
                ReleaseDate = viewModel.ReleaseDate,
                Title = viewModel.Title,
                Tracks = new List<ReleaseTrack>(viewModel.Tracks),
                VendorId = viewModel.VendorId,
            };
        }

        private void MapToViewModel(Release release)
        {
            ArtistId = release.ArtistId;
            ArtistName = release.ArtistName;
            ArtistViewUrl = release.ArtistViewUrl;
            ArtworkUrl = release.ArtworkUrl;
            BuyUrl = release.BuyUrl;
            CollectionId = release.CollectionId;
            Copyright = release.Copyright;
            Country = release.Country;
            DiscCount = release.DiscCount;
            DiscNumber = release.DiscNumber;
            Genre = release.Genre;
            Id = release.Id;
            IsOriginal = release.IsOriginal;
            IsRemix = release.IsRemix;
            PreviewUrl = release.PreviewUrl;
            Price = release.Price;
            ReleaseDate = release.ReleaseDate;
            Title = release.Title;
            Tracks = release.Tracks;
            VendorId = release.VendorId;
        }
    }
}