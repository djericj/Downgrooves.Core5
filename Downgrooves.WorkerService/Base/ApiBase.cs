using Downgrooves.WorkerService.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Downgrooves.WorkerService.Base
{
    public abstract class ApiBase
    {
        protected readonly AppConfig _appConfig;

        public ApiBase(IOptions<AppConfig> config)
        {
            _appConfig = config.Value;
        }

        protected async Task<string> GetString(string resource)
        {
            using (var webClient = new WebClient())
                return await webClient.DownloadStringTaskAsync(new Uri(resource));
        }

        protected async Task<IRestResponse> ApiGet(string resource)
        {
            var client = new RestClient(_appConfig.ApiUrl);
            client.Authenticator = new JwtAuthenticator(_appConfig.Token);
            var request = new RestRequest(resource);
            return await client.ExecuteGetAsync(request);
        }

        protected async Task<IRestResponse> ApiPost(string resource, object value)
        {
            var client = new RestClient(_appConfig.ApiUrl);
            client.Authenticator = new JwtAuthenticator(_appConfig.Token);
            var request = new RestRequest(resource, Method.POST);
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            var json = JsonConvert.SerializeObject(value, settings);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            return await client.ExecutePostAsync(request);
        }
    }
}