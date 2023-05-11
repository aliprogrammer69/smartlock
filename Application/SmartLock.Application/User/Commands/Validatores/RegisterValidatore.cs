using FluentValidation;

using SmartLock.Domain.Repositories;
using SmartLock.Shared.Abstraction;

namespace SmartLock.Application.User.Commands.Validatores {
    public sealed class RegisterValidatore : AbstractValidator<RegisterUserCommand> {
        private static string[] _Roles = { RoleKeys.System, RoleKeys.User, RoleKeys.Admin };
        public RegisterValidatore(IUserRepository userRepository) {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Password).NotEmpty()
                                    .MaximumLength(128)
                                    .MinimumLength(8);

            RuleFor(x => x.Role).NotEmpty()
                                .Must(r => _Roles.Contains(r))
                                .WithMessage($"Inavalid role. valid roles are {string.Join(',', _Roles)}");

            RuleFor(x => x.UserName).Must(r => Task.Run(async () => !await userRepository.UsernameExistsAsync(r)).Result)
                                    .WithMessage(u => $"User with username `{u.UserName}` is already exists");
        }
    }
}
