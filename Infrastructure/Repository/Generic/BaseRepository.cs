using Microsoft.EntityFrameworkCore;
using Domain.Model.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Infrastructure.Repository.Generic
{
    public abstract class BaseRepository<U> : IGenericRepository where U : DbContext
    {
        private readonly U _context;

        public BaseRepository(U context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<T> GetById<T>(int id, T type = null) where T : BaseEntity
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetAll<T>(T type = null) where T : class
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhere<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<bool> Add<T>(T entity, T type = null) where T : class
        {
            await _context.AddAsync(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> AddRange<T>(IEnumerable<T> entity, T type = null) where T : class
        {
            await _context.AddRangeAsync(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> Update<T>(T entity, T type = null) where T : class
        {
            _context.Update(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> Delete<T>(T entity, T type = null) where T : class
        {
            _context.Remove(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> DeleteRange<T>(IEnumerable<T> entities, T type = null) where T : class
        {
            _context.RemoveRange(entities);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> SoftDelete<T>(T entity, T type = null) where T : SoftDeleteEntity
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.Now;

            _context.Update(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<int> CountAll<T>(T type = null) where T : class
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> CountWhere<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public async Task<bool> Any<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        
    }
}
