using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Entities;
using TrainingApp.Core.Entities.AggregateRoots;

namespace TrainingApp.Core.Interfaces.Repositories
{
    public interface ITrainerRepository : IAggregateRootRepository<Trainer>
    {
        Task InsertTraineeAsync(Trainee trainee);
        Task RemoveTraineeAsync(Trainee trainee);
    }
}
