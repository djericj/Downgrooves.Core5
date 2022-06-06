using Downgrooves.Domain;

namespace Downgrooves.Service.Interfaces
{
    public interface IMediaService
    {
        void AddMedia(string path, MediaFile mediaFile);

        void RemoveMedia(string path);
    }
}