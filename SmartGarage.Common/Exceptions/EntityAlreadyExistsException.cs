namespace SmartGarage.Common.Exceptions
{
    public class EntityAlreadyExistsException : ApplicationException
    {
        public EntityAlreadyExistsException(string? message) : base(message) { }
    }    
}
