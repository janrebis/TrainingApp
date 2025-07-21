using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Entities.Enum;

namespace TrainingApp.Core.ValueObjects
{
    public class ExerciseData
    {
        public string Name { get; init; }
        public string? Description { get; init; }
        public int? Sets { get; init; }
        public int? Repetitions { get; init; }
        public TimeSpan? ExerciseDuration { get; init; }
        public int? Weight { get; init; }
        public WeightUnit? WeightUnit { get; init; }
    }
}
