using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Downgrooves.Persistence
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public virtual void Add(T entity) => _context.Set<T>().Add(entity);

        public virtual void AddRange(IEnumerable<T> entities) => _context.Set<T>().AddRange(entities);

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate);

        public T Get(int id) => _context.Set<T>().Find(id);

        public IEnumerable<T> GetAll() => _context.Set<T>().ToList();

        public IEnumerable<T> GetAll(IQueryable<T> query, PagingParameters parameters)
        {
            return query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();
        }

        public void Remove(T entity) => _context.Set<T>().Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

        public T Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
            return entity;
        }

        public void UpdateState(T entity) => _context.Entry(entity).State = EntityState.Modified;
    }
}