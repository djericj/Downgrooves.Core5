using System;
using System.Collections.Generic;
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
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        T Get(int id);
        Task<T> GetAsync(int id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        void UpdateState(T entity);
    }
}