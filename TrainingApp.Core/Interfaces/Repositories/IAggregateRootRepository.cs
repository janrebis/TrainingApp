using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Entities.AggregateRoots;

namespace TrainingApp.Core.Interfaces.Repositories
{
    public interface IAggregateRootRepository<T> where T : class, IAggregateRoot
    {
        Task<T> FindByIdAsync(Guid id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(Guid id);
        Task CommitAsync();
    }
}
