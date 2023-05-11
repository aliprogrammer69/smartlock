using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SmartLock.Infrastructure.DataBase {
    public sealed class SqlServerInitializer {
        private readonly IServiceProvider _serviceProvider;

        public SqlServerInitializer(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public void AutoMigrate() {
            SmartLockDbContext dbContext = _serviceProvider.CreateScope().ServiceProvider.GetService<SmartLockDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
