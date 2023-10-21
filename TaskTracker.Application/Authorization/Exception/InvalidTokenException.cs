namespace TaskTracker.Application.Authorization.Exception
{
    public class InvalidTokenException : System.Exception
    {
        public InvalidTokenException() { }

        public InvalidTokenException(string message) : base(message)
        {
            
        }
    }
}
