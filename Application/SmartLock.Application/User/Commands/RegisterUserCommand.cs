using MediatR;

using SmartLock.Application.User.Dtos;
using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.User.Commands {
    public record RegisterUserCommand(string UserName, string Password, string Role) : IRequest<Result<AuthenticationResult>>;
}
