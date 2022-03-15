﻿using Downgrooves.Persistence.Interfaces;
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

        public void Add(T entity) => _context.Set<T>().Add(entity);

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public void AddRange(IEnumerable<T> entities) => _context.Set<T>().AddRange(entities);

        public async Task AddRangeAsync(IEnumerable<T> entities) => await _context.Set<T>().AddRangeAsync(entities);

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _context.Set<T>().Where(predicate);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().Where(predicate).ToListAsync();

        public T Get(int id) => _context.Set<T>().Find(id);

        public async Task<T> GetAsync(int id) => await _context.Set<T>().FindAsync(id);

        public IEnumerable<T> GetAll() => _context.Set<T>().ToList();

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public void Remove(T entity) => _context.Set<T>().Remove(entity);

        public void RemoveRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

        public void UpdateState(T entity) => _context.Entry(entity).State = EntityState.Modified;
    }
}