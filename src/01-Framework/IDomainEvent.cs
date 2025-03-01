using MediatR;

namespace App.Framework;

public interface IDomainEvent: INotification
{
    Guid EventId => Guid.NewGuid();
    string EventType => GetType().AssemblyQualifiedName!;
    DateTime OccurredOn => DateTime.UtcNow;

}
