using Downgrooves.Domain;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Service.Interfaces
{
    public interface IMixService : IApiService<Mix>
    {
        Task AddMixArtwork(int id, MediaFile mediaFile, string endpoint, CancellationToken token = default);

        Task DeleteMixArtwork(int id, string endpoint, CancellationToken token = default);

        Task AddMixAudio(int id, string endpoint, CancellationToken token = default);

        Task AddAudioChunk(int fragment, MultipartFormDataContent content, string endpoint, CancellationToken token = default);

        Task DeleteMixAudio(int id, string endpoint, CancellationToken token = default);
    }
}