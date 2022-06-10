using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Services.Interfaces
{
    public interface IYouTubeService
    {
        void Process();

        Task<IEnumerable<Video>> GetExistingVideos();

        Task<IEnumerable<Video>> GetYouTubeVideosJson();

        Task<int> AddNewVideos(IEnumerable<Video> videos);

        Task UpdateVideo(Video video);
    }
}