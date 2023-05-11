using SmartLock.Domain.Entities;
using SmartLock.Domain.Models;

namespace SmartLock.Domain.Repositories {
    public interface ITransactionRepository {
        Task AddRangeAsync(IEnumerable<Transaction> transactions);
        Task<TransactionResult> GetAll(ulong lockId, int first = 20, int after = 0);
    }
}
