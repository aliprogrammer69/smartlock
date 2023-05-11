using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Events.User {
    public sealed record UserEdited(Entities.User user, DateTime editDate) : IDomainEvent;
}
