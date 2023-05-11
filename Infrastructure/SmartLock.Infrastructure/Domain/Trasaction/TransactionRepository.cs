using Microsoft.EntityFrameworkCore;

using Entities = SmartLock.Domain.Entities;
using SmartLock.Domain.Models;
using SmartLock.Domain.Repositories;
using SmartLock.Infrastructure.DataBase;

namespace SmartLock.Infrastructure.Domain.Trasaction {
    public sealed class TransactionRepository : AbstractRepository<Entities.Transaction>, ITransactionRepository {

        public TransactionRepository(SmartLockDbContext dbContext) : base(dbContext) {
        }

        public Task AddRangeAsync(IEnumerable<Entities.Transaction> transactions) =>
            _dbContext.transactions.AddRangeAsync(transactions);

        public async Task<TransactionResult> GetAll(ulong lockId, int first = 20, int after = 0) {
            ulong totalCount = (ulong)await _dbContext.transactions.Where(t => t.LockId == lockId).LongCountAsync();
            IEnumerable<Entities.Transaction> transactions = null;
            if (totalCount > 0)
                transactions = await _dbContext.transactions.Include(p => p.Lock)
                                                            .Include(p => p.User)
                                                            .Include(p => p.User.Role)
                                                            .Where(t => t.LockId == lockId)
                                                            .Skip(after)
                                                            .Take(first)
                                                            .OrderBy(t => t.CreateDate)
                                                            .ToListAsync();

            return new TransactionResult(transactions, totalCount, first, after);
        }
    }
}
