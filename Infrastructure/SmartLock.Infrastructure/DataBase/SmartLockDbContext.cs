using Microsoft.EntityFrameworkCore;

using SmartLock.Domain.Entities;

namespace SmartLock.Infrastructure.DataBase {
    public sealed class SmartLockDbContext : DbContext {
        public DbSet<Lock> Locks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Transaction> transactions { get; set; }

        public SmartLockDbContext(DbContextOptions options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartLockDbContext).Assembly);
        }
    }
}
