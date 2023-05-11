using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.Lock {
    public record Locked(Entities.Lock Lock, Entities.User User, bool Success) : IDomainEvent;
}
