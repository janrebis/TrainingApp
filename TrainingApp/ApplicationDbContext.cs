using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Core.Entities;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Infrastructure.Identity;

namespace TrainingApp.Infrastructure
{
    public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
                         IdentityUserClaim<Guid>,
                         IdentityUserRole<Guid>,
                         IdentityUserLogin<Guid>,
                         IdentityRoleClaim<Guid>,
                         IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Trainer> Trainers { get; set; }
        public virtual DbSet<Trainee> Trainees { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<TrainingPlan> TrainingPlans { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Trainer>(builder =>
            {
                builder.HasKey(t => t.TrainerId);

                builder
                    .HasMany(t => t.Trainees)
                    .WithOne(t => t.Trainer)
                    .HasForeignKey(t => t.TrainerId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Trainee>(builder =>
            {
                builder.HasKey(t => t.TraineeId);
                builder.Property(t => t.RowVersion)
                    .IsRowVersion();  
            });

            modelBuilder.Entity<TrainingPlan>(builder =>
            {
                builder.HasKey(tp => tp.TrainingPlanId);
                builder.HasMany(tp => tp.Exercises)
                    .WithOne(e => e.TrainingPlan)
                    .HasForeignKey(e => e.TrainingPlanId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Exercise>(builder =>
            {
                builder.HasKey(e => e.ExerciseId);
            });


        }
    }

}
