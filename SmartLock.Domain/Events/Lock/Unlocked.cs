using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.Lock {
    public record Unlocked(Entities.Lock Lock, Entities.User User, bool Success) : IDomainEvent;
}
