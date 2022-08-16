using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Service
{
    public class ApiServiceBase
    {
        private HttpClient _httpClient;
        private AppConfig _appConfig;

        public ApiServiceBase(IOptions<AppConfig> config, HttpClient httpClient)
        {
            _appConfig = config.Value;
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string path, object data = null, CancellationToken cancel = default)
            => await ExecuteAsync<T>(HttpMethod.Get, path, data, cancel: cancel);

        public async Task<T> PostAsync<T>(string path, object data = null, CancellationToken cancel = default)
            => await ExecuteAsync<T>(HttpMethod.Post, path, data, cancel);

        public async Task<T> PutAsync<T>(string path, object data = null, CancellationToken cancel = default)
            => await ExecuteAsync<T>(HttpMethod.Put, path, data, cancel);

        public async Task<T> DeleteAsync<T>(string path, object data = null, CancellationToken cancel = default)
            => await ExecuteAsync<T>(HttpMethod.Delete, path, data, cancel: cancel);

        private async Task<T> ExecuteAsync<T>(HttpMethod method, string path, object data = null, CancellationToken cancel = default)
        {
            //Console.WriteLine($"{_appConfig.ApiUrl}{path}");
            var request = new HttpRequestMessage(method, path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _appConfig.Token);
            if (data != null) request.Content =
                    new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(json);
            if (response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
            else
            {
                var obj = JsonConvert.DeserializeObject<ApiException>(json);
                throw new Exception($"API Exception: {obj.Status} {obj.Title}");
            }
        }

        public class ApiException
        {
            public string Type { get; set; }
            public string Title { get; set; }
            public string Status { get; set; }
            public string TracsId { get; set; }
        }
    }
}