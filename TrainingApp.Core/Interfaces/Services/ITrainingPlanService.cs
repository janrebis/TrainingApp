using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.ValueObjects;

namespace TrainingApp.Core.Interfaces.Services
{
    public interface ITrainingPlanService
    {
        public Task<Guid> AddTrainingPlan(TrainingPlanData trainingPlanData);
        public Task<IEnumerable<T>> GetTrainingPlans<T>(Guid traineeId);

        public Task<Guid> EditTrainingPlan(TrainingPlan trainingPlan);

        public Task<T> FindTrainingPlanById<T>(Guid trianingPlanId);
    }
}
