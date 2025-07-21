using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.Interfaces.Repositories;
using TrainingApp.Core.Interfaces.Services;
using TrainingApp.Core.ValueObjects;
using TrainingApp.Infrastructure.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrainingApp.Application.Services
{
    public class TrainingPlanService : ITrainingPlanService
    {
        private readonly ITrainingPlanRepository _trainingPlanRepository;

        public TrainingPlanService(ITrainingPlanRepository trainingPlanRepository)
        {
            _trainingPlanRepository = trainingPlanRepository;
        }
        public async Task<Guid> AddTrainingPlan(TrainingPlanData trainingPlanData)
        {
            var trainingPlan = new TrainingPlan(trainingPlanData.Name);
            trainingPlan.AssignTrainee(trainingPlanData.TraineeId);

            if (trainingPlanData.TrainingType.HasValue)
            { 
                trainingPlan.ChangeTrainingType(trainingPlanData.TrainingType.Value); 
            }

            if (trainingPlanData.ScheduledDate.HasValue)
            {
                trainingPlan.Schedule(trainingPlanData.ScheduledDate);
            }

            if (!string.IsNullOrWhiteSpace(trainingPlanData.Notes))
            {
                trainingPlan.UpdateNotes(trainingPlanData.Notes);
            }

            await _trainingPlanRepository.InsertAsync(trainingPlan);
            await _trainingPlanRepository.CommitAsync();
            return trainingPlan.TrainingPlanId;

        }

        public async Task<IEnumerable<TrainingPlan>> GetTrainingPlans(Guid traineeId)
        {
            var trainingPlanList = await _trainingPlanRepository.GetAllTraineePlans(traineeId);
            return trainingPlanList;
        }
    }
}
