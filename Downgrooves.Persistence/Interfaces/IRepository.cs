using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Downgrooves.Persistence.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        T Get(int id);

        IEnumerable<T> GetAll();

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}