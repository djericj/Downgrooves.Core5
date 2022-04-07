using Downgrooves.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IYouTubeService
    {
        Task<IEnumerable<Video>> GetExistingVideos();

        Task<IEnumerable<Video>> GetYouTubeVideosJson();

        Task<int> AddNewVideos(IEnumerable<Video> videos);
    }
}