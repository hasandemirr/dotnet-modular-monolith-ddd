namespace ModularMonolith.Application.Abstractions.Exceptions;

public sealed class ConflictException : DomainException
{
    public ConflictException(string message) : base(message) { }
}