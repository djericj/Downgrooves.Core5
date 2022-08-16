using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Presentation.ViewModels
{
    public class MixTrackViewModel : BaseViewModel, IViewModel
    {
        private IApiService<MixTrack> _mixTrackService;

        [Required(ErrorMessage = null)]
        public string Artist { get; set; }

        [Required(ErrorMessage = null)]
        public string Label { get; set; }

        [Required(ErrorMessage = null)]
        public int MixId { get; set; }

        [Required(ErrorMessage = null)]
        public int Number { get; set; }

        public string Remix { get; set; }

        [Required(ErrorMessage = null)]
        public string Title { get; set; }

        public MixTrackViewModel(IApiService<MixTrack> mixTrackService)
        {
            _mixTrackService = mixTrackService;
        }

        public async Task Add()
        {
            var mix = CreateMixTrack(this);
            MapToViewModel(await _mixTrackService.Add(mix, ApiEndpoint.MixTrack));
        }

        public async Task Update()
        {
            var mix = CreateMixTrack(this);
            MapToViewModel(await _mixTrackService.Update(mix, ApiEndpoint.MixTrack));
        }

        public async Task Remove(int id)
        {
            await _mixTrackService.Remove(id, ApiEndpoint.MixTrack);
        }

        private MixTrack CreateMixTrack(MixTrackViewModel mixTrackViewModel)
        {
            return new MixTrack()
            {
                Artist = mixTrackViewModel.Artist,
                Label = mixTrackViewModel.Label,
                Number = mixTrackViewModel.Number,
                MixId = mixTrackViewModel.MixId,
                Title = mixTrackViewModel.Title,
                Remix = mixTrackViewModel.Remix,
            };
        }

        private void MapToViewModel(MixTrack mixTrack)
        {
            Artist = mixTrack.Artist;
            Label = mixTrack.Label;
            Number = mixTrack.Number;
            MixId = mixTrack.MixId;
            Title = mixTrack.Title;
            Remix = mixTrack.Remix;
        }
    }
}