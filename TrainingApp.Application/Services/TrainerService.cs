﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Application.Exceptions;
using TrainingApp.Core.DTO;
using TrainingApp.Core.Entities;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.Exceptions;
using TrainingApp.Core.RepositoryInterfaces;
using TrainingApp.Core.Validators;
using TrainingApp.Infrastructure.Repositories;

namespace TrainingApp.Application.Services
{
    public class TrainerService
    {
        private readonly ITrainerRepository _trainerRepository;

        public TrainerService(ITrainerRepository trainerRepository)
        {
            _trainerRepository = trainerRepository;
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
            var trainer = await _trainerRepository.FindByIdAsync(trainerId);
            return trainer.GetAllTrainees();
        }

        public async Task<TraineeDTO> GetTraineeByIdAsync(Guid trainerId, Guid traineeId)
        {
            var trainer = await _trainerRepository.FindByIdAsync(trainerId);
            return trainer.GetTraineeData(traineeId);
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
        #endregion
    }
}
