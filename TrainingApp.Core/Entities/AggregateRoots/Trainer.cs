using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingApp.Core.Exceptions;
using TrainingApp.Core.Validators;

namespace TrainingApp.Core.Entities.AggregateRoots
{
    public class Trainer : IAggregateRoot
    {
        public Guid TrainerId { get; private set; }
        public string Name { get; private set; }
        public ICollection<Trainee> Trainees { get; private set; } = new List<Trainee>();
        public Trainer(Guid trainerId, string name)
        {
            TrainerId = trainerId;
            Name = name;
        }

        public void AddTrainee(string name, int age)
        {
            TraineeValidator.ValidateTraineeNameAndAge(name, age);
            if (Trainees.Count >= 10) { throw new MaximumTraineesValueException(); }
            Trainee trainee = new Trainee(name, age);
            Trainees.Add(trainee);
        }

        public bool UpdateTrainee(Guid traineeId, string? name, int? age)
        {
            var traineeToUpdate = GetTraineeById(traineeId);

            if (traineeToUpdate == null) { return false; }
            TraineeValidator.ValidateTraineeNameAndAge(name, age);
            traineeToUpdate.Update(name, age);
            return true;
        }

        public void DeleteTrainee(Guid traineeId)
        {
            var traineeToDelete = GetTraineeById(traineeId);
            Trainees.Remove(traineeToDelete);
        }

        public Trainee GetTraineeData(Guid traineeId)
        {
            var trainee = GetTraineeById(traineeId);
            return trainee;
        }

        public Trainee GetTraineeById(Guid traineeId)
        {
            var trainee = Trainees.FirstOrDefault(temp => temp.TraineeId == traineeId);
            if (trainee == null) throw new InvalidOperationException("Trainee not found.");
            return trainee;
        }

        public IEnumerable<Trainee> GetAllTrainees()
        {
            return Trainees.Where(t => t.TrainerId == TrainerId);
        }

    }
}
