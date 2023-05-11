using FluentValidation;

namespace SmartLock.Application.User.Commands.Validatores {
    public sealed class LoginValidatore : AbstractValidator<LoginUserCommand> {
        public LoginValidatore() {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Password).NotEmpty()
                                    .MaximumLength(128)
                                    .MinimumLength(8);
        }
    }
}
