using Entities = SmartLock.Domain.Entities;
using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.Lock {
    public record UserAccessToLockGranted(Entities.Lock @lock, Entities.User user) : IDomainEvent;

}
