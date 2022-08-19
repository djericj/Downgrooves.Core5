using Downgrooves.Admin.Shared;
using Downgrooves.Domain;
using Downgrooves.Admin.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Pages
{
    public class IndexModel : BaseComponent
    {
        [Inject]
        public IndexViewModel IndexViewModel { get; set; }

        protected IEnumerable<Log> logs = new List<Log>();

        protected override async Task OnInitializedAsync()
        {
            logs = IndexViewModel.GetLogs();
            await base.OnInitializedAsync();
        }
    }
}