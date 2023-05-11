using FluentValidation;

namespace SmartLock.Application.Transaction.Queries.Validatores {
    public sealed class GetLockTransactionValidatore : AbstractValidator<GetLockTransactionsQuery> {
        public GetLockTransactionValidatore() {
            RuleFor(x => x.LockId).NotEmpty()
                                  .GreaterThan(0U);
        }
    }
}
