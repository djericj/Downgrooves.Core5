using Downgrooves.Domain;
using Downgrooves.Service.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Service
{
    public class ApiService<T> : ApiServiceBase, IApiService<T> where T : class
    {
        public ApiService(IOptions<AppConfig> config, HttpClient httpClient) : base(config, httpClient)
        {
        }

        public async Task<T> Add(T entity, string endpoint, CancellationToken token = default)
        {
            return await PostAsync<T>(endpoint, entity, cancel: token);
        }

        public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities, string endpoint, CancellationToken token = default)
        {
            return await PostAsync<IEnumerable<T>>(endpoint, entities, cancel: token);
        }

        public async Task<T> Get(int id, string endpoint, CancellationToken token = default)
        {
            return await GetAsync<T>($"{endpoint}/{id}", cancel: token);
        }

        public async Task<IEnumerable<T>> GetAll(string endpoint, CancellationToken token = default)
        {
            return await GetAsync<IEnumerable<T>>(endpoint, cancel: token);
        }

        public async Task<T> Remove(int id, string endpoint, CancellationToken token = default)
        {
            return await DeleteAsync<T>($"{endpoint}/{id}", id, cancel: token);
        }

        public async Task<T> Update(T entity, string endpoint, CancellationToken token = default)
        {
            return await PutAsync<T>(endpoint, entity, cancel: token);
        }

        public async Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities, string endpoint, CancellationToken token = default)
        {
            return await PutAsync<IEnumerable<T>>(endpoint, entities, cancel: token);
        }
    }
}