using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.Entities.Enum;
using TrainingApp.Core.Validators;

namespace TrainingApp.Core.Entities
{
    public class Exercise
    {
        public Guid ExerciseId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public int? Sets { get; private set; }
        public int? Repetitions { get; private set; }
        public TimeSpan? ExerciseDuration { get; private set; }
        public int? Weight { get; private set; }
        public WeightUnit? WeightUnit { get; private set; }
        public Guid TrainingPlanId { get; set; }
        public TrainingPlan TrainingPlan { get; private set; }
        public Exercise(string name, string? description, int? sets, int? repetitions, TimeSpan? exerciseDuration, int? weight, WeightUnit? weightUnit, Guid trainingPlanId)
        {
            ExerciseId = Guid.NewGuid();
            ExerciseValidator.EnsureNonNegative(sets, nameof(sets));
            ExerciseValidator.EnsureNonNegative(repetitions, nameof(repetitions));
            ExerciseValidator.EnsureNonNegative(weight, nameof(weight));

            Name = name;
            Description = description;
            Sets = sets;
            Repetitions = repetitions;
            ExerciseDuration = exerciseDuration;
            Weight = weight;
            WeightUnit = weightUnit;
            TrainingPlanId = trainingPlanId;
        }

        internal void UpdateDescription(string? description) => Description = description;
        internal void UpdateSets(int? sets)
        {
            Sets = sets;
        }
        internal void UpdateRepetitions(int? repetitions)
        {
            Repetitions = repetitions;
        }
        internal void UpdateExerciseDuration(TimeSpan? exerciseDuration) => ExerciseDuration = exerciseDuration;
        internal void UpdateWeight(int? weight)
        {
            Weight = weight;
        }

        internal void UpdateWeightUnit(WeightUnit? weightUnit)
        {
            WeightUnit = weightUnit;
        }

    }
}
