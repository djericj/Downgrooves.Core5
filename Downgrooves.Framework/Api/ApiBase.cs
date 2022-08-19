using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;

namespace Downgrooves.Framework.Api
{
    public abstract class ApiBase
    {
        private readonly ILogger _logger;

        public ApiBase(ILogger logger)
        {
            _logger = logger;
        }

        public static string GetString(string resource)
        {
            using var webClient = new WebClient();
            return webClient.DownloadString(new Uri(resource));
        }

        public static RestResponse ApiGet(Uri uri, string token)
        {
            var client = new RestClient(uri.GetLeftPart(UriPartial.Authority))
            {
                Authenticator = new JwtAuthenticator(token)
            };
            var request = new RestRequest(uri);
            var response = client.ExecuteGetAsync(request).GetAwaiter().GetResult();
            return response;
        }

        public static RestResponse ApiPost<T>(Uri uri, string token, object value)
        {
            var request = CreateRequest<T>(uri, Method.Post, value);
            var client = new RestClient(uri.GetLeftPart(UriPartial.Authority))
            {
                Authenticator = new JwtAuthenticator(token)
            };
            var response = client.ExecutePostAsync(request).GetAwaiter().GetResult();
            return response;
        }

        public RestResponse<T> ApiPut<T>(Uri uri, string token, object value)
        {
            return ExecuteRequest<T>(uri, token, Method.Put, value);
        }

        public RestResponse<T> ApiDelete<T>(Uri uri, string token, object value)
        {
            return ExecuteRequest<T>(uri, token, Method.Delete, value);
        }

        private static RestRequest CreateRequest<T>(Uri uri, Method method, object value = null)
        {
            var request = new RestRequest(uri, method);
            request.AddHeader("Content-Type", "application/json");
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            if (value != null)
            {
                var json = JsonConvert.SerializeObject(value, settings);
                request.AddStringBody(json, DataFormat.Json);
            }
            return request;
        }

        private RestResponse<T> ExecuteRequest<T>(Uri uri, string token, Method method, object value = null)
        {
            var request = CreateRequest<T>(uri, method, value);
            _logger.LogInformation($"Executing request {method} {request.Resource}");
            var client = new RestClient(uri.GetLeftPart(UriPartial.Authority))
            {
                Authenticator = new JwtAuthenticator(token)
            };
            var response = client.ExecuteAsync<T>(request).GetAwaiter().GetResult();
            return response;
        }
    }
}