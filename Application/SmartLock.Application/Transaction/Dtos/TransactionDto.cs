using SmartLock.Application.Lock.Dtos;
using SmartLock.Application.User.Dtos;
using SmartLock.Domain;

namespace SmartLock.Application.Transaction.Dtos {
    public sealed record TransactionDto(LockDto Lock, UserDto User, ActionType Action, bool Success, DateTime CreateDate);
}
