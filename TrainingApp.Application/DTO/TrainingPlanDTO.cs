using TrainingApp.Core.Entities.Enum;

namespace TrainingApp.Application.DTO
{
    public class TrainingPlanDTO()
    {
       public Guid Id { get; set; }
       public string? Name { get; set; }
       public Guid TraineeId { get; set; }
       public TrainingType? TrainingType { get; set; }
       public DateTime? ScheduledDate { get; set; }
       public string? Notes { get; set; }

    }
}
