using FluentValidation;

namespace SmartLock.Application.Lock.Commands.Validatores {
    public sealed class LockValidatore : AbstractValidator<LockCommand> {
        public LockValidatore() {
            RuleFor(x => x.Id).GreaterThan(0U);
        }
    }
}
