using MediatR;

using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.Lock.Commands {
    public sealed record GrantLockAccessToUserCommand(ulong UserId, ulong LockId) : IRequest<Result>;
}
