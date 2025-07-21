namespace TrainingApp.Core.Exceptions
{
    public class ExerciseNotFoundException : IOException
    {
        public ExerciseNotFoundException() : base("Exercise does not exist in database") { }
    }
}
