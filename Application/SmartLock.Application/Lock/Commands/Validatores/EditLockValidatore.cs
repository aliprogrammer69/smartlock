using FluentValidation;

namespace SmartLock.Application.Lock.Commands.Validatores {
    public sealed class EditLockValidatore : AbstractValidator<EditLockCommand> {
        public EditLockValidatore() {
            RuleFor(r => r.Id).GreaterThan(0U);
            RuleFor(r => r.Name).NotEmpty().MaximumLength(128);
            RuleFor(r => r.Address).NotEmpty().MaximumLength(64);
        }
    }
}
