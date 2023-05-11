using System.Security.Claims;

using MediatR;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Application.User.Dtos;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction;
using SmartLock.Shared.Abstraction.Models;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.User.Commands.Handlers {
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthenticationResult>> {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(IAuthenticationProvider authenticationProvider,
                                          IUserRepository userRepository,
                                          IUnitOfWork unitOfWork) {
            _authenticationProvider = authenticationProvider;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AuthenticationResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken) {
            if (!_authenticationProvider.IsValidToken(request.AccessToken, out ClaimsPrincipal principal) ||
                !ulong.TryParse(principal?.Claims?.FirstOrDefault(x => x.Type == ClaimNames.ClaimNames_UserId)?.Value, out ulong userId))
                return new(ResultCode.Forbidden, null);

            Entities.User user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
                Result<AuthenticationResult>.BadRequest(ErrorMessages.UserNotFound);

            if (!string.Equals(request.RefreshToken, user.RefreshToken))
                return new(ResultCode.Forbidden, null);

            AuthenticationResult result = new(_authenticationProvider.GenerateAccessToken(user),
                                               _authenticationProvider.GenerateRefreshToken());

            user.Login(result.RefreshToken.Value, result.AccessToken.Expires);
            _userRepository.Edit(user);
            await _unitOfWork.CommitAsync();
            return new(result);
        }
    }
}
