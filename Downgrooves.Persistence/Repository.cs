using Downgrooves.Domain;
using Downgrooves.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public virtual async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public virtual void AddRange(IEnumerable<T> entities) => _context.Set<T>().AddRange(entities);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await _context.Set<T>().AddRangeAsync(entities);

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate);

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().Where(predicate).ToListAsync();

        public T Get(int id) => _context.Set<T>().Find(id);

        public IEnumerable<T> GetAll() => _context.Set<T>().ToList();

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(IQueryable<T> query, PagingParameters parameters)
        {
            return await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        public async Task<T> GetAsync(int id) => await _context.Set<T>().FindAsync(id);

        public Task Remove(T entity) => Task.Run(() => _context.Set<T>().Remove(entity));

        public Task RemoveRange(IEnumerable<T> entities) => Task.Run(() => _context.Set<T>().RemoveRange(entities));

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