namespace SmartGarage.Common.Exceptions
{
    public class DuplicateEntityFoundException : ApplicationException
    {
        public DuplicateEntityFoundException(string? message) : base(message) { }
    }
}
