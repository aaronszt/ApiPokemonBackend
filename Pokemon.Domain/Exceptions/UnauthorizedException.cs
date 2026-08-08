namespace Pokemon.Domain.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
    public UnauthorizedException() : base("Not authorized to perform this operation.") { }
}