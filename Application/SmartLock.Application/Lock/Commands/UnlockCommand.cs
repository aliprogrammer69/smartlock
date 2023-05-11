using MediatR;

using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.Lock.Commands {
    public record UnlockCommand(ulong lockId) : IRequest<Result>;
}
