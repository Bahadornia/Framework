namespace App.Framework.DDD;

public interface IAggregateRoot
{
    IDomainEvent[] ClearDomainEvents();
    void AddDomainEvents(IDomainEvent domainEvent);
}
