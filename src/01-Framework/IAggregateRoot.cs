namespace App.Framework;

public interface IAggregateRoot
{
    IDomainEvent[] ClearDomainEvents();
    void AddDomainEvents(IDomainEvent domainEvent);
}
