using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Presentation.ViewModels
{
    public class MixViewModel : BaseViewModel, IViewModel
    {
        private IMixService _mixService;

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

        public MixViewModel(IMixService mixService)
        {
            _mixService = mixService;
        }

        public async Task Add()
        {
            var mix = CreateMix(this);
            MapToViewModel(await _mixService.Add(mix, ApiEndpoint.Mix));
        }

        public async Task AddArtwork(MediaFile mediaFile)
        {
            await _mixService.AddMixArtwork(MixId, mediaFile, ApiEndpoint.Mix);
            await GetMix(MixId);
        }

        public async Task AddAudio()
        {
            await _mixService.AddMixAudio(MixId, ApiEndpoint.Mix);
            await GetMix(MixId);
        }

        public async Task AddAudioChunk(int fragment, MultipartFormDataContent content)
        {
            await _mixService.AddAudioChunk(fragment, content, ApiEndpoint.Mix);
        }

        public async Task<IEnumerable<Mix>> GetMixes()
        {
            return await _mixService.GetAll(ApiEndpoint.Mixes);
        }

        public async Task GetMix(int id)
        {
            MapToViewModel(await _mixService.Get(id, ApiEndpoint.Mix));
        }

        public async Task Update()
        {
            var mix = CreateMix(this);
            MapToViewModel(await _mixService.Update(mix, ApiEndpoint.Mix));
        }

        public async Task Remove(int id)
        {
            await _mixService.Remove(id, ApiEndpoint.Mix);
        }

        public async Task RemoveArtwork()
        {
            await _mixService.DeleteMixArtwork(MixId, ApiEndpoint.Mix);
            ArtworkUrl = null;
            await Update();
        }

        public async Task RemoveAudio()
        {
            await _mixService.DeleteMixAudio(MixId, ApiEndpoint.Mix);
            AudioUrl = null;
            await Update();
        }

        private Mix CreateMix(MixViewModel mixViewModel)
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
                MixId = mixViewModel.MixId,
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
            MixId = mix.MixId;
            ShortDescription = mix.ShortDescription;
            Show = mix.Show == 1;
            Title = mix.Title;
            Tracks = mix.Tracks;
        }
    }
}