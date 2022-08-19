using Downgrooves.Admin.Shared;
using Downgrooves.Domain;
using Downgrooves.Admin.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Pages.Mixes
{
    public class MixesModel : BaseComponent
    {
        protected IEnumerable<Mix> mixes = new List<Mix>();

        [Inject]
        public MixViewModel MixViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            BreadcrumbItems.Add(new Breadcrumb($"/mixes", "Mixes"));
            mixes = MixViewModel.GetMixes();
            await base.OnInitializedAsync();
        }
    }
}