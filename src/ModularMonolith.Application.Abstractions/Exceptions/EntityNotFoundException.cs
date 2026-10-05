namespace ModularMonolith.Application.Abstractions.Exceptions;

public sealed class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} not found. Key: {key}") { }

    public EntityNotFoundException(string message)
        : base(message) { }
}