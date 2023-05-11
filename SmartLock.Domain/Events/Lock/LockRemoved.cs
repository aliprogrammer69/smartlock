using SmartLock.Domain.Entities;
using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.Lock
{
    public record LockRemoved(Entities.Lock item) : IDomainEvent;
}
