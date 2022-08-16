using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Downgrooves.Admin.Service.Interfaces
{
    public interface IApiService<T> where T : class
    {
        Task<T> Add(T entity, string endpoint, CancellationToken token = default);

        Task<IEnumerable<T>> AddRange(IEnumerable<T> entities, string endpoint, CancellationToken token = default);

        Task<T> Get(int id, string endpoint, CancellationToken token = default);

        Task<IEnumerable<T>> GetAll(string endpoint, CancellationToken token = default);

        Task<T> Remove(int id, string endpoint, CancellationToken token = default);

        Task<T> Update(T entity, string endpoint, CancellationToken token = default);

        Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entity, string endpoint, CancellationToken token = default);
    }
}