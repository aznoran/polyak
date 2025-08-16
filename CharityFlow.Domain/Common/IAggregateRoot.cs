using MediatR;

namespace CharityFlow.Domain.Common;

public interface IAggregateRoot
{
    IReadOnlyCollection<INotification> DomainEvents { get; }
    void AddDomainEvent(INotification domainEvent);
    void ClearDomainEvents();
} 