using Microsoft.AspNetCore.Components;
using Blazored.Toast.Services;
using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using Microsoft.JSInterop;
using Downgrooves.Admin.ViewModels.Interfaces;

namespace Downgrooves.Admin.Shared
{
    public class BaseComponent : ComponentBase
    {
        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int? Id { get; set; }

        public bool IsNew { get => !Id.HasValue; }
        public IViewModel ViewModel { get; set; }
        public IJSRuntime JsRuntime { get; set; }
        public string RedirectUrl { get; set; }

        public List<Breadcrumb> BreadcrumbItems { get; set; } = new();

        public async Task Save()
        {
            try
            {
                if (IsNew)
                    ViewModel.Add();
                else
                    ViewModel.Update();

                ToastService.ShowSuccess($"{(IsNew ? "Added" : "Updated")} successfully.");
                var json = JsonConvert.SerializeObject(ViewModel);
                Console.WriteLine(json);

                await OnInitializedAsync();

                if (IsNew)
                    NavigationManager.NavigateTo(RedirectUrl);
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task Delete()
        {
            try
            {
                if (!await JsRuntime.InvokeAsync<bool>("confirm", new object[] { "Are you sure you want to delete this item?" }))
                    return;

                ViewModel.Remove(Id.Value);
                NavigationManager.NavigateTo(RedirectUrl);
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static string FormatRelativeTime(DateTime dateTimeValue)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dateTimeValue.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
    }
}