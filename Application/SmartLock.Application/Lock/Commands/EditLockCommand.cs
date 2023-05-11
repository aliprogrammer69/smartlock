using MediatR;

using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.Lock.Commands {
    public record EditLockCommand(ulong Id, string Name, string Address, bool IsPublic) : IRequest<Result>;
}
