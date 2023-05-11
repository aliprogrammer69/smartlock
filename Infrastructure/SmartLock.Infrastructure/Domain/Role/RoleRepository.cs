using Microsoft.EntityFrameworkCore;

using SmartLock.Domain.Repositories;
using SmartLock.Infrastructure.DataBase;

using Entities = SmartLock.Domain.Entities;

namespace SmartLock.Infrastructure.Domain.Role {
    public sealed class RoleRepository : IRoleRepository {
        private readonly SmartLockDbContext _dbContext;

        public RoleRepository(SmartLockDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Entities.Role>> FindAll(IEnumerable<string> keys) {
            return await _dbContext.Roles.Where(r => keys.Any(k => k == r.Key))
                                         .ToListAsync();
        }

        public async Task<IEnumerable<Entities.Role>> GetAll() =>
            await _dbContext.Roles.ToListAsync();

        public Task<Entities.Role> GetByKey(string key) =>
            _dbContext.Roles.FirstOrDefaultAsync(r => r.Key == key);
    }
}
