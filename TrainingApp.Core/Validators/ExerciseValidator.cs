using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Entities.Enum;
using TrainingApp.Core.Exceptions;
using TrainingApp.Core.ValueObjects;

namespace TrainingApp.Core.Validators
{
    public static class ExerciseValidator
    {
        public static void ValidateExerciseData(ExerciseData data)
        {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }
            if (string.IsNullOrWhiteSpace(data.Name)) { throw new ArgumentException("Exercise name cannot be empty or whitespace", nameof(data.Name)); }
            if (data.Weight.HasValue && !data.WeightUnit.HasValue) { throw new WeightUnitException(nameof(WeightUnit)); }
            EnsureNonNegative(data.Sets, nameof(data.Sets));
            EnsureNonNegative(data.Repetitions, nameof(data.Repetitions));
            EnsureNonNegative(data.Weight, nameof(data.Weight));
        }
        internal static void EnsureNonNegative(int? value, string paramName)
        {
            if (value.HasValue && value < 0)
                throw new NegativeExerciseValueException(paramName);
        }

    }
}
