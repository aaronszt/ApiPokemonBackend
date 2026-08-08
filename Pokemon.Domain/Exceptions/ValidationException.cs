namespace Pokemon.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
    public ValidationException(IEnumerable<string> errors) 
        : base("One or more validation errors have occurred.")
    {
        Errors = errors;
    }
    public IEnumerable<string>? Errors { get; }
}