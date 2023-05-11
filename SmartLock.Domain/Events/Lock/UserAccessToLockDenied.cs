using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.Lock {
    public record UserAccessToLockDenied(Entities.Lock @lock, Entities.User user) : IDomainEvent;

}
