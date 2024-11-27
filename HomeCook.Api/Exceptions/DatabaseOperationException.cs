namespace HomeCook.Api.Exceptions
{
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
