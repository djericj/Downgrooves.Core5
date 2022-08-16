using Downgrooves.Domain;
using Downgrooves.Admin.Service.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Service
{
    public class MixService : ApiService<Mix>, IMixService
    {
        public MixService(IOptions<AppConfig> config, HttpClient httpClient) : base(config, httpClient)
        {
        }

        public async Task AddMixArtwork(int id, MediaFile mediaFile, string endpoint, CancellationToken token = default)
        {
            await PostAsync<Mix>($"{endpoint}/{id}/artwork", mediaFile, cancel: token);
        }

        public async Task DeleteMixArtwork(int id, string endpoint, CancellationToken token = default)
        {
            await DeleteAsync<Mix>($"{endpoint}/{id}/artwork", cancel: token);
        }

        public async Task AddMixAudio(int id, string endpoint, CancellationToken token = default)
        {
            await PostAsync<Mix>($"{endpoint}/{id}/audio", cancel: token);
        }

        public async Task AddAudioChunk(int fragment, MultipartFormDataContent content, string endpoint, CancellationToken token = default)
        {
            await PostAsync<Mix>($"{endpoint}/audio/appendfile/{fragment}", content, cancel: token);
        }

        public async Task DeleteMixAudio(int id, string endpoint, CancellationToken token = default)
        {
            await DeleteAsync<Mix>($"{endpoint}/{id}/audio", cancel: token);
        }
    }
}