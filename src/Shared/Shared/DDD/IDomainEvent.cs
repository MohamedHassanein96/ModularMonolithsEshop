using MediatR;

namespace Shared.DDD;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    public DateTime OccuredOn { get; }
    public string EventType { get; }
}
