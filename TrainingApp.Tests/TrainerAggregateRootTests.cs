using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.Exceptions;

namespace TrainingApp.Tests
{
    public class TrainerTests
    {
        [Fact]
        public void AddTrainee_ValidData_AddsTraineeToCollection()
        {
            // Arrange
            var trainer = new Trainer(Guid.NewGuid(), "Jan Kowalski");

            // Act
            trainer.AddTrainee("Anna Nowak", 25);

            // Assert
            trainer.Trainees.Should().HaveCount(1);
            trainer.Trainees.First().Name.Should().Be("Anna Nowak");
            trainer.Trainees.First().Age.Should().Be(25);
        }

        [Fact]
        public void AddTrainee_NullName_ThrowsArgumentNullException()
        {
            // Arrange
            var trainer = new Trainer(Guid.NewGuid(), "Jan Kowalski");

            // Act & Assert
            Action act = () => trainer.AddTrainee(null, 25);
            act.Should().Throw<ArgumentNullException>()
               .WithMessage("*Name should not be empty or null*");
        }

        [Fact]
        public void AddTrainee_NegativeAge_ThrowsArgumentException()
        {
            // Arrange
            var trainer = new Trainer(Guid.NewGuid(), "Jan Kowalski");

            // Act & Assert
            Action act = () => trainer.AddTrainee("Anna Nowak", -5);
            act.Should().Throw<ArgumentException>()
               .WithMessage("Age cannot be negative*");
        }

        [Fact]
        public void AddTrainee_MoreThanTenTrainees_ThrowsInvalidOperationException()
        {
            // Arrange
            var trainer = new Trainer(Guid.NewGuid(), "Jan Kowalski");
            for (int i = 0; i < 10; i++)
            {
                trainer.AddTrainee($"Trainee {i}", 20 + i);
            }

            // Act & Assert
            Action act = () => trainer.AddTrainee("Extra Trainee", 30);
            act.Should().Throw<MaximumTraineesValueException>()
               .WithMessage("Cannot add more than 10 trainees.");
        }

        [Fact]
        public void UpdateTrainee_ValidData_UpdatesTrainee()
        {
            // Arrange
            var trainer = new Trainer(Guid.NewGuid(), "Jan Kowalski");
            trainer.AddTrainee("Anna Nowak", 25);
            var traineeId = trainer.Trainees.First().TraineeId;

            // Act
            trainer.UpdateTrainee(traineeId, "Anna Kowalska", 26);

            // Assert
            var updatedTrainee = trainer.GetTraineeData(traineeId);
            updatedTrainee.Name.Should().Be("Anna Kowalska");
            updatedTrainee.Age.Should().Be(26);
        }

        [Fact]
        public void UpdateTrainee_NonExistingTrainee_ThrowsInvalidOperationException()
        {
            // Arrange
            var trainer = new Trainer(Guid.NewGuid(), "Jan Kowalski");

            // Act & Assert
            Action act = () => trainer.UpdateTrainee(Guid.NewGuid(), "Anna", 25);
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Trainee not found.");
        }

        [Fact]
        public void DeleteTrainee_ExistingTrainee_RemovesTrainee()
        {
            // Arrange
            var trainer = new Trainer(Guid.NewGuid(), "Jan Kowalski");
            trainer.AddTrainee("Anna Nowak", 25);
            var traineeId = trainer.Trainees.First().TraineeId;

            // Act
            trainer.DeleteTrainee(traineeId);

            // Assert
            trainer.Trainees.Should().BeEmpty();
        }

        [Fact]
        public void DeleteTrainee_NonExistingTrainee_ThrowsInvalidOperationException()
        {
            // Arrange
            var trainer = new Trainer(Guid.NewGuid(), "Jan Kowalski");

            // Act & Assert
            Action act = () => trainer.DeleteTrainee(Guid.NewGuid());
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Trainee not found.");
        }

        [Fact]
        public void GetTraineeById_ExistingTrainee_ReturnsTrainee()
        {
            // Arrange
            var trainer = new Trainer(Guid.NewGuid(), "Jan Kowalski");
            trainer.AddTrainee("Anna Nowak", 25);
            var traineeId = trainer.Trainees.First().TraineeId;

            // Act
            var trainee = trainer.GetTraineeData(traineeId);

            // Assert
            trainee.Should().NotBeNull();
            trainee.TraineeId.Should().Be(traineeId);
            trainee.Name.Should().Be("Anna Nowak");
        }
    }
}

