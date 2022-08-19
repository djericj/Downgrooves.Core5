using Downgrooves.Domain;
using Downgrooves.Admin.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Downgrooves.Admin.ViewModels.Interfaces;

namespace Downgrooves.Admin.ViewModels
{
    public class MixViewModel : BaseViewModel, IViewModel
    {
        private readonly IApiService<Mix> _mixService;

        [Required(ErrorMessage = null)]
        public string Artist { get; set; }

        [Required(ErrorMessage = null)]
        public string ArtworkUrl { get; set; }

        public string Category { get; set; }
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = null)]
        public string Description { get; set; }

        [Required(ErrorMessage = null)]
        public Genre Genre { get; set; }

        public int MixId { get; set; }

        [Required(ErrorMessage = null)]
        public string Length { get; set; }

        [Required(ErrorMessage = null)]
        public string AudioUrl { get; set; }

        [Required(ErrorMessage = null)]
        public string Title { get; set; }

        [Required(ErrorMessage = null)]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = null)]
        public bool Show { get; set; }

        public ICollection<MixTrack> Tracks { get; set; }

        public MixViewModel(IApiService<Mix> mixService)
        {
            _mixService = mixService;
        }

        public void Add()
        {
            var mix = CreateMix(this);
            MapToViewModel(_mixService.Add(mix, ApiEndpoint.Mix));
        }

        public void AddArtwork(MediaFile mediaFile)
        {
            GetMix(MixId);
        }

        public IEnumerable<Mix> GetMixes()
        {
            return _mixService.GetAll(ApiEndpoint.Mixes);
        }

        public void GetMix(int id)
        {
            MapToViewModel(_mixService.Get(id, ApiEndpoint.Mix));
        }

        public void Update()
        {
            var mix = CreateMix(this);
            MapToViewModel(_mixService.Update(mix, ApiEndpoint.Mix));
        }

        public void Remove(int id)
        {
            _mixService.Remove(id, ApiEndpoint.Mix);
        }

        public void RemoveAudio()
        {
            Update();
        }

        private static Mix CreateMix(MixViewModel mixViewModel)
        {
            return new Mix()
            {
                Artist = mixViewModel.Artist,
                ArtworkUrl = GetFileNameFromUrl(mixViewModel.ArtworkUrl),
                AudioUrl = GetFileNameFromUrl(mixViewModel.AudioUrl),
                Category = mixViewModel.Category,
                CreateDate = mixViewModel.CreateDate,
                Description = mixViewModel.Description,
                Genre = mixViewModel.Genre,
                Length = mixViewModel.Length,
                Id = mixViewModel.MixId,
                ShortDescription = mixViewModel.ShortDescription,
                Show = mixViewModel.Show ? 1 : 0,
                Title = mixViewModel.Title,
                Tracks = mixViewModel.Tracks,
            };
        }

        private void MapToViewModel(Mix mix)
        {
            Artist = mix.Artist;
            ArtworkUrl = mix.ArtworkUrl;
            AudioUrl = mix.AudioUrl;
            Category = mix.Category;
            CreateDate = mix.CreateDate;
            Description = mix.Description;
            Genre = mix.Genre;
            Length = mix.Length;
            MixId = mix.Id;
            ShortDescription = mix.ShortDescription;
            Show = mix.Show == 1;
            Title = mix.Title;
            Tracks = mix.Tracks;
        }
    }
}