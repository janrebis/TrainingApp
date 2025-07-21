using Microsoft.EntityFrameworkCore;
using TrainingApp.Application.DTO;
using TrainingApp.Application.Exceptions;
using TrainingApp.Core.Entities;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.Exceptions;
using TrainingApp.Core.Interfaces.Repositories;
using TrainingApp.Core.Interfaces.Services;
using TrainingApp.Core.Validators;
using AutoMapper;

namespace TrainingApp.Application.Services
{
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerRepository _trainerRepository;
        private readonly IMapper _mapper;

        public TrainerService(ITrainerRepository trainerRepository, IMapper mapper)
        {
            _trainerRepository = trainerRepository;
            _mapper = mapper;
        }

        #region Trainer
        public async Task<Guid> AddTrainer(Guid trainerId, string name)
        {
            var trainer = new Trainer(trainerId, name);
            await _trainerRepository.InsertAsync(trainer);
            await _trainerRepository.CommitAsync();
            return trainer.TrainerId;
        }

        public async Task<Trainer> FindTrainerAsync(Guid trainerId)
        {
            return await _trainerRepository.FindByIdAsync(trainerId);
        }

        public async Task RemoveTrainerAsync(Guid trainerId)
        {
            await _trainerRepository.RemoveAsync(trainerId);
            await _trainerRepository.CommitAsync();
        }
        #endregion

        #region Trainee
        public async Task AddTraineeAsync(Guid trainerId, string name, int age)
        {
            TraineeValidator.ValidateTraineeNameAndAge(name, age);

          
            var trainer = await _trainerRepository.FindByIdAsync(trainerId);
            if (trainer == null)
                throw new InvalidOperationException("Trainer not found.");

            
            if (trainer.Trainees.Count >= 10)
                throw new MaximumTraineesValueException();

            
            var trainee = new Trainee(name, age) { TrainerId = trainerId };

            trainer.Trainees.Add(trainee);
            await _trainerRepository.InsertTraineeAsync(trainee);
            await _trainerRepository.CommitAsync();
        }

        public async Task UpdateTraineeAsync(Guid trainerId, Guid traineeId, string? name, int? age)
        {
            var trainer = await _trainerRepository.FindByIdAsync(trainerId);

            if (trainer == null)
                throw new EntityNotFoundException("Trainer not found.");

            var updated = trainer.UpdateTrainee(traineeId, name, age);
            if (!updated)
                throw new EntityNotFoundException("Trainee not found.");

            try
            {
                await _trainerRepository.UpdateAsync(trainer);
                await _trainerRepository.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException("Trainee was modified by another user. Please reload and try again.");
            }
        }
        public async Task<IEnumerable<TraineeDTO>> GetAllTraineesAsync(Guid trainerId)
        {
            IEnumerable<TraineeDTO> traineesDTO = new List<TraineeDTO>();   
            var trainer = await _trainerRepository.FindByIdAsync(trainerId);
            foreach(Trainee trainee in trainer.GetAllTrainees())
            {
                traineesDTO.Append(_mapper.Map<TraineeDTO>(trainee));
            };

            return traineesDTO;

        }

        public async Task<TraineeDTO> GetTraineeByIdAsync(Guid trainerId, Guid traineeId)
        {
            var trainer = await _trainerRepository.FindByIdAsync(trainerId);
            return _mapper.Map<TraineeDTO>(trainer.GetTraineeById(traineeId));
        }

        public async Task RemoveTraineeAsync(Guid trainerId, Guid traineeId)
        {
            var trainer = await _trainerRepository.FindByIdAsync(trainerId);
            var trainee = trainer.Trainees.FirstOrDefault(t => t.TraineeId == traineeId);

            if (trainee != null) 
            { 
            trainer.DeleteTrainee(traineeId);
            //TODO: Jak zrobić, żeby na pewno usuwało go z listy i z db, transakcja?
            await _trainerRepository.RemoveTraineeAsync(trainee);
            await _trainerRepository.UpdateAsync(trainer);
            await _trainerRepository.CommitAsync();
            }
        }


        Task<Trainee> ITrainerService.GetTraineeByIdAsync(Guid trainerId, Guid traineeId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Trainee>> ITrainerService.GetAllTraineesAsync(Guid trainerId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
