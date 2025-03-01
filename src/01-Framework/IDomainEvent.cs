using MediatR;

namespace Framework;

public interface IDomainEvent: INotification
{
    Guid EventId => Guid.NewGuid();
    string EventType => GetType().AssemblyQualifiedName;
    DateTime OccurredOn => DateTime.UtcNow;

}
