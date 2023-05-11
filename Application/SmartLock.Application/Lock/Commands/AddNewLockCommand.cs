using MediatR;

using SmartLock.Shared.Abstraction.Models;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.Lock.Commands {
    public record AddNewLockCommand(string Name, string Address, bool IsPublic = false) : IRequest<Result<Entities.Lock>>;
}
