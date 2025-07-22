using System;
using System.Linq;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.Entities.Enum;
using TrainingApp.Core.Exceptions;
using TrainingApp.Core.ValueObjects;
using Xunit;

namespace TrainingApp.Tests.Domain
{
    public class TrainingPlanTests
    {
        [Fact]
        public void Constructor_ShouldInitializeCorrectly()
        {
            var plan = new TrainingPlan("My Plan");

            Assert.NotEqual(Guid.Empty, plan.TrainingPlanId);
            Assert.Equal("My Plan", plan.Name);
            Assert.Equal(TrainingPlanStatus.ACTIVE, plan.Status);
            Assert.True((DateTime.UtcNow - plan.CreationDate).TotalSeconds < 5);
        }

        [Fact]
        public void Constructor_ShouldUseDefaultName_WhenNameIsNull()
        {
            var plan = new TrainingPlan(null);

            Assert.Equal("Training Plan", plan.Name);
        }

        //[Fact]
        //public void AddTrainingPlanToId_ShouldSetTraineeId()
        //{
        //    var plan = new TrainingPlan("Test");
        //    var id = Guid.NewGuid();

        //    plan.AddTrainingPlanToId(id);

        //    Assert.Equal(id, plan.TraineeId);
        //}

        [Fact]
        public void ChangeTrainingType_ShouldUpdateType()
        {
            var plan = new TrainingPlan("Test");
            plan.ChangeTrainingType(TrainingType.CARDIO);

            Assert.Equal(TrainingType.CARDIO, plan.TrainingType);
        }

        [Fact]
        public void ChangeStatus_ShouldUpdateStatus()
        {
            var plan = new TrainingPlan("Test");
            plan.ChangeStatus(TrainingPlanStatus.INACTIVE);

            Assert.Equal(TrainingPlanStatus.INACTIVE, plan.Status);
        }

        [Fact]
        public void UpdateNotes_ShouldUpdateNoteText()
        {
            var plan = new TrainingPlan("Test");
            plan.UpdateNotes("Note 123");

            Assert.Equal("Note 123", plan.Notes);
        }

        [Fact]
        public void Schedule_ShouldSetValidDate()
        {
            var plan = new TrainingPlan("Test");
            var validDate = plan.CreationDate.AddHours(1);

            plan.Schedule(validDate);

            Assert.Equal(validDate, plan.ScheduledDate);
        }

        [Fact]
        public void Schedule_ShouldThrow_WhenScheduledDateIsBeforeCreation()
        {
            var plan = new TrainingPlan("Test");
            var invalidDate = plan.CreationDate.AddMinutes(-10);

            Assert.Throws<ArgumentException>(() => plan.Schedule(invalidDate));
        }

        [Fact]
        public void AddExercise_ShouldAddValidExercise()
        {
            var plan = new TrainingPlan("Test");

            var data = new ExerciseData
            {
                Name = "Push-ups",
                Sets = 3,
                Repetitions = 10
            };

            plan.AddExercise(data);

            var exercise = plan.Exercises.First();
            Assert.Equal("Push-ups", exercise.Name);
            Assert.Equal(3, exercise.Sets);
            Assert.Single(plan.Exercises);
        }

        [Fact]
        public void AddExercise_ShouldThrow_WhenWeightUnitMissingButWeightGiven()
        {
            var plan = new TrainingPlan("Test");

            var data = new ExerciseData
            {
                Name = "Squat",
                Weight = 100,
                Sets = 3,
                Repetitions = 8
            };

            Assert.Throws<WeightUnitException>(() => plan.AddExercise(data));
        }

        [Fact]
        public void AddExercise_ShouldThrow_WhenNameIsEmpty()
        {
            var plan = new TrainingPlan("Test");

            var data = new ExerciseData
            {
                Name = " ",
                Sets = 3
            };

            Assert.Throws<ArgumentException>(() => plan.AddExercise(data));
        }

        [Fact]
        public void UpdateExercise_ShouldModifyExistingExercise()
        {
            var plan = new TrainingPlan("Test");
            var data = new ExerciseData
            {
                Name = "Row",
                Sets = 2,
                Repetitions = 12
            };
            plan.AddExercise(data);

            var existingId = plan.Exercises.First().ExerciseId;

            var newData = new ExerciseData
            {
                Name = "Row",
                Sets = 4,
                Repetitions = 10,
                Description = "Updated"
            };

            plan.UpdateExercise(existingId, newData);

            var updated = plan.GetExerciseById(existingId);
            Assert.Equal("Updated", updated.Description);
            Assert.Equal(4, updated.Sets);
            Assert.Equal(10, updated.Repetitions);
        }

        [Fact]
        public void GetExerciseById_ShouldReturnExercise_WhenExists()
        {
            var plan = new TrainingPlan("Test");
            var data = new ExerciseData { Name = "Deadlift", Sets = 5 };
            plan.AddExercise(data);
            var id = plan.Exercises.First().ExerciseId;

            var result = plan.GetExerciseById(id);

            Assert.NotNull(result);
            Assert.Equal("Deadlift", result.Name);
        }

        [Fact]
        public void GetExerciseById_ShouldThrow_WhenNotExists()
        {
            var plan = new TrainingPlan("Test");

            Assert.Throws<ExerciseNotFoundException>(() => plan.GetExerciseById(Guid.NewGuid()));
        }

        [Fact]
        public void DeleteExercise_ShouldRemoveExercise()
        {
            var plan = new TrainingPlan("Test");
            var data = new ExerciseData { Name = "Pull-ups", Sets = 4 };
            plan.AddExercise(data);
            var id = plan.Exercises.First().ExerciseId;

            plan.DeleteExercise(id);

            Assert.Empty(plan.Exercises);
        }
    }
}