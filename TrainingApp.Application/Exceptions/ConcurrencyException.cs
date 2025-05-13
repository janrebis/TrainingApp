namespace TrainingApp.Application.Exceptions
{
    [Serializable]
    internal class ConcurrencyException : Exception
    {
        public ConcurrencyException()
        {
        }

        public ConcurrencyException(string? message) : base(message)
        {
        }

        public ConcurrencyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}