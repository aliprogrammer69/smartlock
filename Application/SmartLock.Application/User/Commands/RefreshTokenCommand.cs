using MediatR;

using SmartLock.Application.User.Dtos;
using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.User.Commands {
    public sealed record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<Result<AuthenticationResult>>;
}
