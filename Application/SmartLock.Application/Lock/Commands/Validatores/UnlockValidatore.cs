using FluentValidation;

namespace SmartLock.Application.Lock.Commands.Validatores {
    public sealed class UnlockValidatore : AbstractValidator<UnlockCommand> {
        public UnlockValidatore() {
            RuleFor(x => x.lockId).GreaterThan(0U);
        }
    }
}
