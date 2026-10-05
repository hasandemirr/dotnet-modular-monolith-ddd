namespace ModularMonolith.Application.Abstractions.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}