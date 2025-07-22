
using TrainingApp.Core.Entities;
using TrainingApp.Core.Entities.AggregateRoots;

namespace TrainingApp.Core.Interfaces.Services
{
    public interface ITrainerService
    {
        public Task<Guid> AddTrainer(Guid trainerId, string name); //tutaj podaje trainerId, bo kopiuje je z Identity, żeby rozdzielic usera od operacji biznesowych
        public Task<Trainer> FindTrainerAsync(Guid trainerId);
        public Task RemoveTrainerAsync(Guid trainerId);
        public Task AddTraineeAsync(Guid trainerId, string name, int age);
        public Task UpdateTraineeAsync(Guid trainerId, Guid traineeId, string? name, int? age);
        public Task<IEnumerable<T>> GetAllTraineesAsync<T>(Guid trainerId) where T : class;
        public Task<T> GetTraineeByIdAsync<T>(Guid trainerId, Guid traineeId) where T : class;
        public Task RemoveTraineeAsync(Guid trainerId, Guid traineeId);


    }
}
