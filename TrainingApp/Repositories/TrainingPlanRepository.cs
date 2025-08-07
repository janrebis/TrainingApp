using TrainingApp.Core.Entities.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Core.Interfaces.Repositories;

namespace TrainingApp.Infrastructure.Repositories
{

    public class TrainingPlanRepository : BaseRepository<TrainingPlan>, ITrainingPlanRepository
    {
        public TrainingPlanRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task<TrainingPlan> FindByIdAsync(Guid id)
        {
            var trainingPlan = await _dbContext.TrainingPlans
                .Include(tp => tp.Exercises)
                .FirstOrDefaultAsync(temp => temp.TrainingPlanId == id);


            if (trainingPlan == null)
            {
                throw new Exception($"Training plan with id {id} not found.");
            }

            return trainingPlan;
        }

        public async Task<IEnumerable<TrainingPlan>> GetAllTraineePlans(Guid id)
        {
            var trainingPlansList =  await _dbContext.TrainingPlans.Where(temp => temp.TraineeId == id).ToListAsync();
            return trainingPlansList;
        }
    }
}
