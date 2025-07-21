
namespace TrainingApp.Core.Exceptions
{
    public class NegativeExerciseValueException : IOException
    {
        public NegativeExerciseValueException(string paramName) : base($"Value of {paramName} cannot be negative") { }
    }
}
