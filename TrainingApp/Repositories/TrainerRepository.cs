using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Core.Entities;
using TrainingApp.Core.Entities.AggregateRoots;
using TrainingApp.Core.RepositoryInterfaces;
using TrainingApp.Infrastructure.Repositories.RepositoryExceptions;

namespace TrainingApp.Infrastructure.Repositories
{

    public class TrainerRepository : BaseRepository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task<Trainer> FindByIdAsync(Guid id)
        {
            var trainer = await _dbContext.Trainers
                .Include(t => t.Trainees)
                .FirstOrDefaultAsync(tr => tr.TrainerId == id);

            if (trainer == null)
                throw new TrainerNotFoundException(); 

            return trainer;
        }

        public async Task InsertTraineeAsync(Trainee trainee)
        {
            await _dbContext.Trainees.AddAsync(trainee);
        }

        public async Task RemoveTraineeAsync(Trainee trainee)
        {
            _dbContext.Trainees.Remove(trainee);
        }

    }
}
