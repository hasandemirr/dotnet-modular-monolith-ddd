using ModularMonolith.Application.Abstractions.Events;

namespace ModularMonolith.Application.Abstractions.Domain;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events;

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _events.Add(domainEvent);

    public void ClearDomainEvents() => _events.Clear();
}