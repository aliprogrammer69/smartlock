using FluentValidation;

namespace SmartLock.Application.Lock.Commands.Validatores {
    public sealed class GrantLockAccessToUserValidatore : AbstractValidator<GrantLockAccessToUserCommand> {
        public GrantLockAccessToUserValidatore() {
            RuleFor(x => x.UserId).GreaterThan(0U);
            RuleFor(x => x.LockId).GreaterThan(0U);
        }
    }
}
