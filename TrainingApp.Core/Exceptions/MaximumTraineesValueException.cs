using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingApp.Core.Exceptions
{
    public class MaximumTraineesValueException : IOException
    {
        public MaximumTraineesValueException() : base("Cannot add more than 10 trainees.") { }
    }
}
