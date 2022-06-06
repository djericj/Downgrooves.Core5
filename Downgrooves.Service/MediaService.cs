using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using System.IO;

namespace Downgrooves.Service
{
    public class MediaService : IMediaService
    {
        public void AddMedia(string path, MediaFile mediaFile)
        {
            if (mediaFile.Data != null)
            {
                using var writer = new BinaryWriter(File.OpenWrite(path));
                writer.Write(mediaFile.Data);
            }
        }

        public void RemoveMedia(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}