using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseAndTrainingPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingPlans",
                columns: table => new
                {
                    TrainingPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TraineeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingType = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlans", x => x.TrainingPlanId);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sets = table.Column<int>(type: "int", nullable: true),
                    Repetitions = table.Column<int>(type: "int", nullable: true),
                    ExerciseDuration = table.Column<TimeSpan>(type: "time", nullable: true),
                    Weight = table.Column<int>(type: "int", nullable: true),
                    WeightUnit = table.Column<int>(type: "int", nullable: true),
                    TrainingPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExerciseId);
                    table.ForeignKey(
                        name: "FK_Exercises_TrainingPlans_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlans",
                        principalColumn: "TrainingPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_TrainingPlanId",
                table: "Exercises",
                column: "TrainingPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "TrainingPlans");
        }
    }
}
