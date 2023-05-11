using MediatR;

using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.Lock.Commands {
    public record LockCommand(ulong Id) : IRequest<Result>;
}
