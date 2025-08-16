using MediatR;

namespace CharityFlow.Domain.Common;

public abstract class AggregateRoot : IAggregateRoot
{
    private readonly List<INotification> _domainEvents = new();

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
} 