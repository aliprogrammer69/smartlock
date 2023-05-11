using MediatR;

using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.Lock.Commands {
    public sealed record DenyLockAccessFromUserCommand(ulong UserId, ulong LockId) : IRequest<Result>;
}
