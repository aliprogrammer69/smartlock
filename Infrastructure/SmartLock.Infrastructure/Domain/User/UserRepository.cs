using Microsoft.EntityFrameworkCore;

using SmartLock.Domain.Repositories;
using SmartLock.Infrastructure.DataBase;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Infrastructure.Domain.User {
    public class UserRepository : AbstractRepository<Entities.User>, IUserRepository {
        public UserRepository(SmartLockDbContext dbContext) : base(dbContext) {
        }

        public Task<Entities.User> GetByIdAsync(ulong id) =>
            _dbContext.Users.Include(r => r.Role).FirstOrDefaultAsync(u => u.Id == id);

        public Task<Entities.User> GetByUsernameAsync(string username) =>
            _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserName == username);

        public Task<bool> HasAccessToLock(ulong userId, ulong lockId) =>
            _dbContext.Users.Include(l => l.AccessedLocks)
                            .Where(u => u.Id == userId)
                            .AsNoTracking()
                            .AnyAsync(u => u.AccessedLocks.Any(l => l.Id == lockId));

    }
}
