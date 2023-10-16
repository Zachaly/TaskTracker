namespace TaskTracker.Database.Exception
{
    public class EntityNotFoundException : System.Exception
    {
        public EntityNotFoundException(string entityName) : base($"{entityName} not found")
        {
            
        }
    }
}
