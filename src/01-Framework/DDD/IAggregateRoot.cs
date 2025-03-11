using Microsoft.Extensions.Logging;

namespace App.Framework.DDD;

public interface IAggregateRoot
{
    IReadOnlyList<IDomainEvent> DomainEvents { get;}
    IDomainEvent[] ClearDomainEvents();
    void AddDomainEvents(IDomainEvent domainEvent);
}
