
namespace TrainingApp.Core.Exceptions
{
    public class WeightUnitException : IOException
    {
        public WeightUnitException(string paramName) : base($"Value of {paramName} cannot be empty if weight if provided") { }
    }
}
