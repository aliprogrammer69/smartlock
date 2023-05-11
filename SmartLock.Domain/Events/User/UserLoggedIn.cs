using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.User {
    public record UserLoggedIn(Entities.User user, DateTime logginDate) : IDomainEvent;
}
