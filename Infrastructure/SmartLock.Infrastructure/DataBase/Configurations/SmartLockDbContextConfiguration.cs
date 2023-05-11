using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using SmartLock.Infrastructure.DataBase.Configurations.Options;

namespace SmartLock.Infrastructure.DataBase.Configurations {
    public static class SmartLockDbContextConfiguration {
        private static readonly MemoryCache _memoryCache = new (new MemoryCacheOptions());
        public static readonly Action<DbContextOptionsBuilder> ConfigInMemoryDbContext = (opt) => {
            opt.UseInMemoryDatabase("SmartLock");
        };

        public static readonly Action<DbContextOptionsBuilder, DataBaseOptions> ConfigSqlServerDbContext = (optionBuilder, dbOptions) => {
            optionBuilder.UseMemoryCache(_memoryCache);
            optionBuilder.UseSqlServer(dbOptions.ConnectionString);
        };
    }
}
