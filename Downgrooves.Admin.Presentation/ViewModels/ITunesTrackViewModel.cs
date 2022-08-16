using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Presentation.ViewModels
{
    public class ITunesTrackViewModel : ITunesViewModel
    {
        protected IApiService<ITunesTrack> _service;

        public ITunesTrackViewModel(IApiService<ITunesTrack> service)
        {
            _service = service;
        }

        [Required(ErrorMessage = null)]
        public int TrackId { get; set; }

        [Required(ErrorMessage = null)]
        public string Kind { get; set; }

        [Required(ErrorMessage = null)]
        public string TrackName { get; set; }

        [Required(ErrorMessage = null)]
        public string TrackCensoredName { get; set; }

        [Required(ErrorMessage = null)]
        public string TrackViewUrl { get; set; }

        [Required(ErrorMessage = null)]
        public string PreviewUrl { get; set; }

        [Required(ErrorMessage = null)]
        public string ArtworkUrl30 { get; set; }

        [Required(ErrorMessage = null)]
        public double TrackPrice { get; set; }

        [Required(ErrorMessage = null)]
        public string TrackExplicitness { get; set; }

        [Required(ErrorMessage = null)]
        public int DiscCount { get; set; }

        [Required(ErrorMessage = null)]
        public int DiscNumber { get; set; }

        [Required(ErrorMessage = null)]
        public int TrackNumber { get; set; }

        [Required(ErrorMessage = null)]
        public int TrackTimeMillis { get; set; }

        [Required(ErrorMessage = null)]
        public string IsStreamable { get; set; }

        public async Task AddTrack()
        {
            var track = CreateTrack(this);
            MapToViewModel(await _service.Add(track, ApiEndpoint.ITunesTrack));
        }

        public async Task<IEnumerable<ITunesTrack>> GetTracks()
        {
            return await _service.GetAll(ApiEndpoint.ITunesTracks);
        }

        public async Task GetTrack(int id)
        {
            MapToViewModel(await _service.Get(id, ApiEndpoint.ITunesTrack));
        }

        public async Task UpdateTrack()
        {
            var track = CreateTrack(this);
            MapToViewModel(await _service.Update(track, ApiEndpoint.ITunesTrack));
        }

        public async Task Remove(int id)
        {
            await _service.Remove(id, ApiEndpoint.ITunesTrack);
        }

        private ITunesTrack CreateTrack(ITunesTrackViewModel viewModel)
        {
            return new ITunesTrack()
            {
                ArtistId = viewModel.ArtistId,
                ArtistName = viewModel.ArtistName,
                ArtistViewUrl = viewModel.ArtistViewUrl,
                ArtworkUrl100 = viewModel.ArtworkUrl100,
                ArtworkUrl60 = viewModel.ArtworkUrl60,
                ArtworkUrl30 = viewModel.ArtworkUrl30,
                DiscCount = viewModel.DiscCount,
                DiscNumber = viewModel.DiscNumber,
                Kind = viewModel.Kind,
                PreviewUrl = viewModel.PreviewUrl,
                IsStreamable = viewModel.IsStreamable,
                TrackCensoredName = viewModel.TrackCensoredName,
                TrackExplicitness = viewModel.TrackExplicitness,
                TrackId = viewModel.TrackId,
                TrackName = viewModel.TrackName,
                TrackNumber = viewModel.TrackNumber,
                TrackPrice = viewModel.TrackPrice,
                TrackTimeMillis = viewModel.TrackTimeMillis,
                TrackViewUrl = viewModel.TrackViewUrl,
                CollectionCensoredName = viewModel.CollectionCensoredName,
                CollectionExplicitness = viewModel.CollectionExplicitness,
                CollectionId = viewModel.CollectionId,
                CollectionName = viewModel.CollectionName,
                CollectionPrice = viewModel.CollectionPrice,
                CollectionViewUrl = viewModel.CollectionViewUrl,
                Country = viewModel.Country,
                Currency = viewModel.Currency,
                Id = viewModel.Id,
                TrackCount = viewModel.TrackCount,
                PrimaryGenreName = viewModel.PrimaryGenreName,
                ReleaseDate = viewModel.ReleaseDate,
                WrapperType = viewModel.WrapperType,
            };
        }

        private void MapToViewModel(ITunesTrack track)
        {
            ArtistId = track.ArtistId;
            ArtistName = track.ArtistName;
            ArtistViewUrl = track.ArtistViewUrl;
            ArtworkUrl100 = track.ArtworkUrl100;
            ArtworkUrl60 = track.ArtworkUrl60;
            ArtworkUrl30 = track.ArtworkUrl30;
            DiscCount = track.DiscCount;
            DiscNumber = track.DiscNumber;
            Kind = track.Kind;
            PreviewUrl = track.PreviewUrl;
            IsStreamable = track.IsStreamable;
            TrackCensoredName = track.TrackCensoredName;
            TrackExplicitness = track.TrackExplicitness;
            TrackId = track.TrackId;
            TrackName = track.TrackName;
            TrackNumber = track.TrackNumber;
            TrackPrice = track.TrackPrice;
            TrackTimeMillis = track.TrackTimeMillis;
            TrackViewUrl = track.TrackViewUrl;
            CollectionCensoredName = track.CollectionCensoredName;
            CollectionExplicitness = track.CollectionExplicitness;
            CollectionId = track.CollectionId;
            CollectionName = track.CollectionName;
            CollectionPrice = track.CollectionPrice;
            CollectionViewUrl = track.CollectionViewUrl;
            Country = track.Country;
            Currency = track.Currency;
            Id = track.Id;
            TrackCount = track.TrackCount;
            PrimaryGenreName = track.PrimaryGenreName;
            ReleaseDate = track.ReleaseDate;
            WrapperType = track.WrapperType;
        }
    }
}