using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Entities.Enum;

namespace TrainingApp.Core.ValueObjects
{
    public record TrainingPlanData(string? Name, Guid TraineeId, TrainingType? TrainingType, DateTime? ScheduledDate, string? Notes);
}
