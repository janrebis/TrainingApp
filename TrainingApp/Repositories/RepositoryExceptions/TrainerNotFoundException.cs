using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingApp.Infrastructure.Repositories.RepositoryExceptions
{
    public class TrainerNotFoundException : IOException
    {
        public TrainerNotFoundException() : base("Trainer not found in database.") { }
    }
}
