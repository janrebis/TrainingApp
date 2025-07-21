using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Entities.Enum;
using TrainingApp.Core.Exceptions;
using TrainingApp.Core.Validators;
using TrainingApp.Core.ValueObjects;

namespace TrainingApp.Core.Entities.AggregateRoots
{
    public class TrainingPlan : IAggregateRoot
    {
        public Guid TrainingPlanId { get; private set; }
        public string Name { get; private set; }
        public Guid TraineeId { get; private set; }
        public TrainingType? TrainingType { get; private set; }
        public ICollection<Exercise> Exercises { get; set; } =   new List<Exercise>();
        public TimeSpan? TotalDuration
        {
            get => Exercises?.Aggregate(TimeSpan.Zero, (sum, exercise) => sum + (exercise.ExerciseDuration ?? TimeSpan.Zero)) ?? TimeSpan.Zero;
        }
        public TrainingPlanStatus Status { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime? ScheduledDate { get; private set; }
        public string? Notes { get; private set; }
        private static readonly TrainingPlanStatus DefaultStatus = TrainingPlanStatus.ACTIVE;
        private static readonly string DefaultName = "Training Plan";
        private TrainingPlan()
        {
            TrainingPlanId = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            Status = DefaultStatus;
        }

        public TrainingPlan(string trainingPlanName)
        {

            TrainingPlanId = Guid.NewGuid();
            Name = string.IsNullOrEmpty(trainingPlanName) ? DefaultName : trainingPlanName;
            Status = DefaultStatus;
            CreationDate = DateTime.UtcNow;
        }

        public void AssignTrainee(Guid traineeId) { TraineeId = traineeId; }
        public void ChangeTrainingType(TrainingType trainingType) { TrainingType = trainingType; }
        public void ChangeStatus(TrainingPlanStatus trainingPlanStatus) { Status = trainingPlanStatus; }
        public void UpdateNotes(string? notes) { Notes = notes; }
        public void Schedule(DateTime? scheduledDate)
        {
            if (scheduledDate.HasValue && scheduledDate.Value < CreationDate)
            {
                throw new ArgumentException("Scheduled date cannot be earlier than creation date");
            }

            ScheduledDate = scheduledDate;
        }



        public void AddExercise(ExerciseData data)
        {
            ExerciseValidator.ValidateExerciseData(data);
            Exercise newExercise = new Exercise(data.Name, data.Description, data.Sets, data.Repetitions, data.ExerciseDuration, data.Weight, data.WeightUnit, TrainingPlanId);
            Exercises.Add(newExercise);
        }

        public void UpdateExercise(Guid exerciseId, ExerciseData data)
        {
            ExerciseValidator.ValidateExerciseData(data);
            var exercise = GetExerciseById(exerciseId);
            if (!string.IsNullOrEmpty(data.Description)) exercise.UpdateDescription(data.Description);
            if (data.Sets.HasValue) exercise.UpdateSets(data.Sets);
            if (data.Repetitions.HasValue) exercise.UpdateRepetitions(data.Repetitions);
            if (data.ExerciseDuration.HasValue) exercise.UpdateExerciseDuration(data.ExerciseDuration);
            if (data.Weight.HasValue) exercise.UpdateWeight(data.Weight);
            if (data.WeightUnit.HasValue) exercise.UpdateWeightUnit(data.WeightUnit);
        }

        public void DeleteExercise(Guid exerciseId)
        {
            var exercise = GetExerciseById(exerciseId);
            Exercises.Remove(exercise);
        }

        public Exercise GetExerciseById(Guid exerciseId)
        {
            var exercise = Exercises.FirstOrDefault(temp => temp.ExerciseId == exerciseId);
            if (exercise == null) { throw new ExerciseNotFoundException(); }
            return exercise;
        }
    }
}
