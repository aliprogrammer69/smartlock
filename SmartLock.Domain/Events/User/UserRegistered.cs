using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.User {
    public record UserRegistered(Entities.User user) : IDomainEvent;
}
