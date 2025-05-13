using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingApp.Core.DTO
{
    public class TraineeDTO
    {
        public Guid TraineeId { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }

        public TraineeDTO() { }

        public TraineeDTO(Guid traineeId, string name, int? age)
        {
            TraineeId = traineeId;
            Name = name;
            Age = age;
        }
    }
}
