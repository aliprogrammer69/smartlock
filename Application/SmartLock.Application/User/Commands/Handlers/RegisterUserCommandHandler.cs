using MediatR;

using SmartLock.Application.Consts;
using SmartLock.Application.Services;
using SmartLock.Application.User.Dtos;
using SmartLock.Domain.Entities;
using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction.Models;
using SmartLock.Shared.Abstraction.Services;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Application.User.Commands.Handlers {
    public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthenticationResult>> {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationProvider _authenticationProvider;

        public RegisterUserCommandHandler(IUserRepository userRepository,
                                          IRoleRepository roleRepository,
                                          ICryptoService cryptoService,
                                          IUnitOfWork unitOfWork,
                                          IAuthenticationProvider authenticationProvider) {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _cryptoService = cryptoService;
            _unitOfWork = unitOfWork;
            _authenticationProvider = authenticationProvider;
        }

        public async Task<Result<AuthenticationResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken) {
            Role role = await _roleRepository.GetByKey(request.Role);
            if (role is null)
                return Result<AuthenticationResult>.BadRequest(ErrorMessages.RoleNotFound);

            string passwordSalt = _cryptoService.GenerateRandomString(16);
            string hashedPass = _cryptoService.Hash(request.Password, passwordSalt);
            Entities.User user = Entities.User.Create(request.UserName, hashedPass, passwordSalt, role);

            await _userRepository.AddAsync(user);
            await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);

            AuthenticationResult result = await _authenticationProvider.LoginAsync(user);
            return new Result<AuthenticationResult>(result);
        }
    }
}
