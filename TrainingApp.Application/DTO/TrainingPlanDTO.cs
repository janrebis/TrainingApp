using TrainingApp.Core.Entities.Enum;

namespace TrainingApp.Api.DTO
{
    public class TrainingPlanDTO()
    {
       public string? Name { get; set; }
       public Guid TraineeId { get; set; }
       public TrainingType? TrainingType { get; set; }
       public DateTime? ScheduledDate { get; set; }
       public string? Notes { get; set; }

    }
}
