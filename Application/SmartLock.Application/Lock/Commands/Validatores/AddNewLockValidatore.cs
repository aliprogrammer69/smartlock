using FluentValidation;

namespace SmartLock.Application.Lock.Commands.Validatores {
    public sealed class AddNewLockValidatore : AbstractValidator<AddNewLockCommand> {
        public AddNewLockValidatore() {
            RuleFor(r => r.Name).NotEmpty().MaximumLength(128);
            RuleFor(r => r.Address).NotEmpty().MaximumLength(64);
        }
    }
}
