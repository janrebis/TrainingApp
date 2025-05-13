using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Entities.AggregateRoots;

namespace TrainingApp.Core.Entities
{
    public class Trainee
    {
        [Key]
        public Guid TraineeId { get; private set; }
        [Required]
        public string Name { get; private set; }
        public int? Age { get; private set; }

        public DateTime CreatedAt { get; }

        public Guid TrainerId { get;  set; }
        public Trainer Trainer { get; private set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }  
        public Trainee(string name, int age)
        {
            TraineeId = Guid.NewGuid();
            Name = name;
            Age = age;
        }
        private Trainee() { }

        internal void Update(string? name, int? age)
        {
            Name = name;
            Age = age;
        }

    }
}
