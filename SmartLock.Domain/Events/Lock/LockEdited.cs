using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.Lock {
    public record LockEdited(Entities.Lock @lock) : IDomainEvent;
}
