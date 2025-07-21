using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Entities.AggregateRoots;

namespace TrainingApp.Core.Interfaces.Repositories
{
    public interface ITrainingPlanRepository : IAggregateRootRepository<TrainingPlan>
    {
        public Task<IEnumerable<TrainingPlan>> GetAllTraineePlans(Guid id);
    }
}
