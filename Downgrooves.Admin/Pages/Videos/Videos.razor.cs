using Downgrooves.Admin.Shared;
using Downgrooves.Domain;
using Downgrooves.Admin.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Pages.Videos
{
    public class VideosModel : BaseComponent
    {
        [Inject]
        public VideoViewModel VideoViewModel { get; set; }

        protected IEnumerable<Video> videos = new List<Video>();

        protected override async Task OnInitializedAsync()
        {
            videos = await VideoViewModel.GetVideos();
            await base.OnInitializedAsync();
        }
    }
}