using Microsoft.EntityFrameworkCore;

using SmartLock.Domain.Repositories;
using SmartLock.Infrastructure.DataBase;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Infrastructure.Domain.Lock {
    public class LockRepository : AbstractRepository<Entities.Lock>, ILockRepository {
        public LockRepository(SmartLockDbContext dbContext) : base(dbContext) {
        }

        public Task<bool> Exists(ulong lockId) =>
            _dbContext.Locks.AsNoTracking()
                            .AnyAsync(l => l.Id == lockId);

        public Task<Entities.Lock> GetAsync(ulong id) =>
            _dbContext.Locks.Include(l => l.AccessedUsers).FirstOrDefaultAsync(l => l.Id == id);

        public async Task<IEnumerable<Entities.Lock>> GetUserLocks(ulong userId) =>
            await _dbContext.Locks.Include(l => l.AccessedUsers)
                            .Where(l => l.AccessedUsers.Any(u => u.Id == userId))
                            .ToListAsync();
    }
}
