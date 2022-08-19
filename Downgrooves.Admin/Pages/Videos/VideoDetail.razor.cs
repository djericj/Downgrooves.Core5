using Downgrooves.Admin.Shared;
using Downgrooves.Admin.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Pages.Videos
{
    public class VideoDetailModel : BaseComponent
    {
        [Parameter]
        public int? VideoId { get; set; }

        [Inject]
        public VideoViewModel VideoViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id.HasValue)
                VideoViewModel.GetVideo(Id.Value);
            await base.OnInitializedAsync();
        }
    }
}