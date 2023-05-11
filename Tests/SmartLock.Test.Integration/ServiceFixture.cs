using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using SmartLock.Bootstrapper.Extensions;

namespace SmartLock.Test.Integration {
    public sealed class ServiceFixture {
        public IServiceProvider ServiceProvider { get; private set; }
        public ServiceFixture() {
            IServiceCollection services = new ServiceCollection();
            services.AddSmartLock()
                .AddSmartLockDbContext(opt => {
                    opt.UseInMemoryDatabase($"SmartLock_{Guid.NewGuid()}");
                }).AddSingleton(p => new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                                                              .AddJsonFile("appsettings.json")
                                                              .Build());

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
