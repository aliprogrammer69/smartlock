using SmartLock.Infrastructure.DataBase;

namespace SmartLock.Infrastructure.Helpers {
    public sealed class SeedHelper {
        private readonly SmartLockDbContext _dbContext;

        public SeedHelper(SmartLockDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void AddRoles() {
            _dbContext.Roles.AddRange(new(1, "Administrator", "admin"),
                                      new(2, "User", "user"),
                                      new(3, "System", "system"));
            _dbContext.SaveChanges();
        }
    }
}
