using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.Lock {
    public record LockAdded(Entities.Lock item) : IDomainEvent;
}
