using MediatR;

using SmartLock.Application.Transaction.Dtos;
using SmartLock.Shared.Abstraction.Models;

namespace SmartLock.Application.Transaction.Queries {
    public sealed record GetLockTransactionsQuery(ulong LockId, int First = 20, int After = 0) : IRequest<Result<TransactionResultDto>>;
}
