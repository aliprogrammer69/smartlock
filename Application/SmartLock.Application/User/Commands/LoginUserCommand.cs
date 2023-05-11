using MediatR;

using SmartLock.Application.User.Dtos;
using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.User.Commands {
    public record LoginUserCommand(string UserName, string Password) : IRequest<Result<AuthenticationResult>>;
}
