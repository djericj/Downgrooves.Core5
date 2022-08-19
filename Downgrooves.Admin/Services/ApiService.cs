using Downgrooves.Admin.Services.Interfaces;
using Downgrooves.Framework.Api;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Downgrooves.Admin.Services
{
    public class ApiService<T> : ApiBase, IApiService<T> where T : class
    {
        public ApiService(IOptions<AppConfig> config, ILogger<ApiService<T>> logger) : base(logger)
        {
            Token = config.Value.Token;
        }

        public string Token { get; private set; }

        public string ApiUrl { get; set; }

        public T Add(T entity, string endpoint)
        {
            var response = ApiPost<T>(GetUri(endpoint), Token, entity);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities, string endpoint)
        {
            var response = ApiPost<IEnumerable<T>>(GetUri(endpoint), Token, entities);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(response.Content);
        }

        public T Get(int id, string endpoint)
        {
            var response = ApiGet(GetUri($"{endpoint}/{id}"), Token);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public IEnumerable<T> GetAll(string endpoint)
        {
            var response = ApiGet(GetUri(endpoint), Token);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(response.Content);
        }

        public T Remove(int id, string endpoint)
        {
            var response = ApiDelete<T>(GetUri($"{endpoint}/{id}"), Token, id);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public T Update(T entity, string endpoint)
        {
            var response = ApiPut<T>(GetUri(endpoint), Token, entity);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public IEnumerable<T> UpdateRange(IEnumerable<T> entities, string endpoint)
        {
            var response = ApiPut<IEnumerable<T>>(GetUri(endpoint), Token, entities);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(response.Content);
        }

        public Uri GetUri(string path)
        {
            return new Uri($"{ApiUrl}/{path}");
        }
    }
}