using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.Interfaces.Repositories;

namespace TrainingApp.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IAggregateRootRepository<T> where T : class, IAggregateRoot
    {
        protected readonly ApplicationDbContext _dbContext;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task InsertAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public virtual Task UpdateAsync(T entity)
        {
            var entry = _dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbContext.Attach(entity);
                entry.State = EntityState.Modified;
            }
            return Task.CompletedTask;
        }

        public virtual async Task RemoveAsync(Guid id)
        {
            var entity = await FindByIdAsync(id);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
            }
        }

        public virtual async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public abstract Task<T> FindByIdAsync(Guid id);
    }
}
