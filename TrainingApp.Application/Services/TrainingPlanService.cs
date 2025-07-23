using AutoMapper;
using TrainingApp.Application.DTO;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.Interfaces.Repositories;
using TrainingApp.Core.Interfaces.Services;
using TrainingApp.Core.ValueObjects;

namespace TrainingApp.Application.Services
{
    public class TrainingPlanService : ITrainingPlanService
    {
        private readonly ITrainingPlanRepository _trainingPlanRepository;
        private readonly IMapper _mapper;
        public TrainingPlanService(ITrainingPlanRepository trainingPlanRepository, IMapper mapper)
        {
            _trainingPlanRepository = trainingPlanRepository;
            _mapper = mapper;
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

        public async Task<IEnumerable<T>> GetTrainingPlans<T>(Guid traineeId)
        {
            if (typeof(T) != typeof(TrainingPlanDTO))
            {
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
            }

            var trainingPlans = await _trainingPlanRepository.GetAllTraineePlans(traineeId);
            var result = trainingPlans
                .Select(trainingPlan => _mapper.Map<T>(trainingPlan))
                .ToList();
            
            return result;
        }
    }
}
