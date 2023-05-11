using MediatR;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Application.User.Dtos;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction;
using SmartLock.Shared.Abstraction.Models;
using SmartLock.Shared.Abstraction.Services;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.User.Commands.Handlers {
    public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthenticationResult>> {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IAuthenticationProvider _authenticationProvider;

        public LoginUserCommandHandler(IUserRepository userRepository,
                                       ICryptoService cryptoService,
                                       IAuthenticationProvider authenticationProvider) {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _authenticationProvider = authenticationProvider;
        }

        public async Task<Result<AuthenticationResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken) {
            Entities.User user = await _userRepository.GetByUsernameAsync(request.UserName);
            if (user is null)
                return new Result<AuthenticationResult>(ResultCode.Forbidden, ErrorMessages.UsernameOrPasswordNotCurrent);

            string hashedPassword = _cryptoService.Hash(request.Password, user.PaswordSalt);
            if (!string.Equals(hashedPassword, user.Password))
                return new Result<AuthenticationResult>(ResultCode.Forbidden, ErrorMessages.UsernameOrPasswordNotCurrent);

            AuthenticationResult result = await _authenticationProvider.LoginAsync(user);
            return new Result<AuthenticationResult>(result);
        }
    }
}