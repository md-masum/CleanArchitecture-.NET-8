using System.Linq.Expressions;
using CleanArchitecture.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity>(ApplicationDbContext context)
        where TEntity : class
    {
        public virtual bool Add(TEntity entity)
        {
            context.Add(entity);
            return SaveChange();
        }
        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            context.Add(entity);
            return await SaveChangeAsync();
        }

        public virtual bool Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            return SaveChange();
        }
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            return await SaveChangeAsync();
        }

        public virtual bool Remove(TEntity entity)
        {
            context.Remove(entity);
            return SaveChange();
        }
        public virtual async Task<bool> RemoveAsync(TEntity entity)
        {
            context.Remove(entity);
            return await SaveChangeAsync();
        }

        public virtual TEntity GetById(int id)
        {
            return context.Set<TEntity>().Find(id) ?? throw new InvalidOperationException();
        }
        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id) ?? throw new InvalidOperationException();
        }

        public virtual ICollection<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }
        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public virtual ICollection<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate).ToList();
        }
        public virtual async Task<ICollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual IQueryable<TEntity> GetAsQueryable()
        {
            return context
                .Set<TEntity>().AsQueryable();
        }

        public virtual bool UpdateRange(ICollection<TEntity> entity)
        {
            context.Set<TEntity>().UpdateRange(entity);
            return SaveChange();
        }
        public virtual async Task<bool> UpdateRangeAsync(ICollection<TEntity> entity)
        {
            context.Set<TEntity>().UpdateRange(entity);
            return await SaveChangeAsync();
        }

        public bool RemoveRange(ICollection<TEntity> entity)
        {
            context.Set<TEntity>().RemoveRange(entity);
            return SaveChange();
        }
        public async Task<bool> RemoveRangeAsync(ICollection<TEntity> entity)
        {
            context.Set<TEntity>().RemoveRange(entity);
            return await SaveChangeAsync();
        }

        private bool SaveChange()
        {
            return context.SaveChanges() > 0;
        }

        private async Task<bool> SaveChangeAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
