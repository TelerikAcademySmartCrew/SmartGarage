namespace SmartGarage.Common.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(string? message) : base(message) { }
    }
}
