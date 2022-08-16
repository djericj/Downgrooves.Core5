using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;

namespace Downgrooves.Admin.Presentation.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected static string GetFileNameFromUrl(string url)
        {
            if (url == null) return null;
            var decoded = HttpUtility.UrlDecode(url);

            if (decoded.IndexOf("?") is { } queryIndex && queryIndex != -1)
                decoded = decoded.Substring(0, queryIndex);

            return Path.GetFileName(decoded);
        }
    }
}