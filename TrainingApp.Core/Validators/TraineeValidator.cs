using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingApp.Core.Validators
{
    public class TraineeValidator
    {
        public static void ValidateTraineeNameAndAge(string name, int? age)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("Name should not be empty or null", nameof(name)); }
            if (age < 0) { throw new ArgumentException("Age cannot be negative"); }
        }
    }
}
