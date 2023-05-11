using FluentValidation;

namespace SmartLock.Application.User.Commands.Validatores {
    public sealed class RefreshTokenValidatore : AbstractValidator<RefreshTokenCommand> {
        public RefreshTokenValidatore() {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
