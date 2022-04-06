using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Interfaces
{
    public interface IArtworkService
    {
        Task GetArtwork(string type);
    }
}