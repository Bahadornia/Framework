namespace App.Framework.DDD;

public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : class
{

    private readonly List<IDomainEvent> events = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => events.AsReadOnly();

    public void AddDomainEvents(IDomainEvent domainEvent)
    {
        events.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        var dequeuedEvents = events.ToArray();
        events.Clear();
        return dequeuedEvents;
    }
}
