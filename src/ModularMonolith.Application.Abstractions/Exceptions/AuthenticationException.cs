namespace ModularMonolith.Application.Abstractions.Exceptions;

public sealed class AuthenticationException : DomainException
{
    public AuthenticationException(string message) : base(message) { }
}