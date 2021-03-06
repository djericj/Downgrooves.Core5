using Downgrooves.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        Task AddAsync(T entity);

        void AddRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        T Get(int id);

        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> GetAllAsync(IQueryable<T> query, PagingParameters parameters);

        Task<T> GetAsync(int id);

        Task Remove(T entity);

        Task RemoveRange(IEnumerable<T> entities);

        T Update(T entity);

        void UpdateState(T entity);
    }
}