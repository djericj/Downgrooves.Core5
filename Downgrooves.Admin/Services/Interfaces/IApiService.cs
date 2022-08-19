using System;
using System.Collections.Generic;

namespace Downgrooves.Admin.Services.Interfaces
{
    public interface IApiService<T> where T : class
    {
        T Add(T entity, string endpoint);

        IEnumerable<T> AddRange(IEnumerable<T> entities, string endpoint);

        T Get(int id, string endpoint);

        IEnumerable<T> GetAll(string endpoint);

        T Remove(int id, string endpoint);

        T Update(T entity, string endpoint);

        IEnumerable<T> UpdateRange(IEnumerable<T> entities, string endpoint);

        Uri GetUri(string path);
    }
}